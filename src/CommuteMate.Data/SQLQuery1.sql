CREATE OR ALTER FUNCTION dbo.fn_GetRideCountBetweenUsers
(
    @UserId1 INT,
    @UserId2 INT
)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;

    SELECT @Count = COUNT(*)
    FROM Bookings b
    INNER JOIN Rides r ON b.RideId = r.Id
    WHERE b.Status = 2
      AND r.Status = 2
      AND (
            (r.PublisherId = @UserId1 AND b.PassengerId = @UserId2)
            OR
            (r.PublisherId = @UserId2 AND b.PassengerId = @UserId1)
          );

    RETURN ISNULL(@Count, 0);
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_IsRecurringEligible
(
    @PassengerId INT,
    @PublisherId INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @RecurringCount INT;

    SELECT @RecurringCount = COUNT(*)
    FROM Bookings b
    INNER JOIN Rides r ON b.RideId = r.Id
    WHERE b.PassengerId = @PassengerId
      AND r.PublisherId = @PublisherId
      AND r.IsRecurring = 1
      AND b.Status = 2
      AND r.Status = 2;

    IF @RecurringCount >= 3
        RETURN 1;

    RETURN 0;
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_CalculateDiscount
(
    @PassengerId INT,
    @PublisherId INT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @RideCount INT;
    DECLARE @LoyaltyDiscount DECIMAL(5,2) = 0;
    DECLARE @RecurringBonus DECIMAL(5,2) = 0;
    DECLARE @TotalDiscount DECIMAL(5,2) = 0;

    SET @RideCount = dbo.fn_GetRideCountBetweenUsers(@PassengerId, @PublisherId);

    SET @LoyaltyDiscount = CASE
        WHEN @RideCount >= 20 THEN 20.00
        WHEN @RideCount >= 10 THEN 15.00
        WHEN @RideCount >= 5  THEN 10.00
        ELSE 0.00
    END;

    IF dbo.fn_IsRecurringEligible(@PassengerId, @PublisherId) = 1
        SET @RecurringBonus = 5.00;

    SET @TotalDiscount = @LoyaltyDiscount + @RecurringBonus;

    IF @TotalDiscount > 25.00
        SET @TotalDiscount = 25.00;

    RETURN @TotalDiscount;
END;
GO

CREATE OR ALTER VIEW dbo.vw_UserActivitySummary
AS
SELECT
    u.Id            AS UserId,
    u.FullName,
    u.AverageRating,
    u.TotalRides,

    (SELECT COUNT(*) FROM Rides r
      WHERE r.PublisherId = u.Id)                        AS RidesPublished,

    (SELECT COUNT(*) FROM Rides r
      WHERE r.PublisherId = u.Id AND r.Status = 0)       AS ActiveRidesPublished,

    (SELECT COUNT(*) FROM Rides r
      WHERE r.PublisherId = u.Id AND r.Status = 2)       AS CompletedRidesPublished,

    (SELECT COUNT(*) FROM Bookings b
      WHERE b.PassengerId = u.Id AND b.Status <> 1)      AS BookingsMade,

    (SELECT COUNT(*) FROM Bookings b
       INNER JOIN Rides r ON b.RideId = r.Id
      WHERE b.PassengerId = u.Id
        AND b.Status = 0
        AND r.RideDate >= CAST(GETUTCDATE() AS date))       AS UpcomingBookings,

    (SELECT COUNT(*) FROM Bookings b
      WHERE b.PassengerId = u.Id AND b.Status = 2)       AS CompletedBookings,

    (SELECT CAST(ISNULL(SUM(b.SeatsBooked * r.PricePerSeat - b.TotalPrice), 0) AS decimal(10,2))
       FROM Bookings b
       INNER JOIN Rides r ON b.RideId = r.Id
      WHERE b.PassengerId = u.Id
        AND b.Status <> 1
        AND b.DiscountAppliedPercent > 0)                AS TotalSavings
FROM Users u
WHERE u.IsActive = 1;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetUserRecentActivity
    @UserId INT,
    @TopN   INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopN)
        a.ActivityType,
        a.RideId,
        a.FromCity,
        a.ToCity,
        a.RideDate,
        a.RideTime,
        a.StatusText,
        a.Amount,
        a.ActivityAtUtc
    FROM
    (
        SELECT
            'Published' AS ActivityType,
            r.Id        AS RideId,
            r.FromCity,
            r.ToCity,
            r.RideDate,
            r.RideTime,
            CASE r.Status
                WHEN 0 THEN 'Active'
                WHEN 1 THEN 'Full'
                WHEN 2 THEN 'Completed'
                WHEN 3 THEN 'Cancelled'
                ELSE 'Unknown'
            END AS StatusText,
            CAST(r.PricePerSeat AS decimal(10,2)) AS Amount,
            r.CreatedAtUtc AS ActivityAtUtc
        FROM Rides r
        WHERE r.PublisherId = @UserId

        UNION ALL

        SELECT
            'Booked' AS ActivityType,
            r.Id     AS RideId,
            r.FromCity,
            r.ToCity,
            r.RideDate,
            r.RideTime,
            CASE b.Status
                WHEN 0 THEN 'Confirmed'
                WHEN 1 THEN 'Cancelled'
                WHEN 2 THEN 'Completed'
                ELSE 'Unknown'
            END AS StatusText,
            CAST(b.TotalPrice AS decimal(10,2)) AS Amount,
            b.BookedAtUtc AS ActivityAtUtc
        FROM Bookings b
        INNER JOIN Rides r ON b.RideId = r.Id
        WHERE b.PassengerId = @UserId
    ) AS a
    ORDER BY a.ActivityAtUtc DESC;
END;