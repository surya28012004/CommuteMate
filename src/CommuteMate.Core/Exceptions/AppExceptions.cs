using System;

namespace CommuteMate.Core.Exceptions;

/// <summary>
/// Base exception type for application-specific errors that carry an HTTP status code.
/// Services can throw these and an exception middleware can translate them to HTTP responses.
/// </summary>
public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }

    /// <summary>
    /// Corresponding HTTP status code for the exception.
    /// </summary>
    public abstract int StatusCode { get; }
}

/// <summary>
/// Thrown when authentication fails (invalid credentials, bad password).
/// Maps to HTTP 401 Unauthorized.
/// </summary>
public class InvalidCredentialsException : AppException
{
    public InvalidCredentialsException(string? message = "Invalid credentials") : base(message ?? "Invalid credentials") { }
    public override int StatusCode => 401;
}

/// <summary>
/// Thrown when a requested resource cannot be found.
/// Maps to HTTP 404 Not Found.
/// </summary>
public class ResourceNotFoundException : AppException
{
    public ResourceNotFoundException(string? message = "Resource not found") : base(message ?? "Resource not found") { }
    public override int StatusCode => 404;
}

/// <summary>
/// Thrown when a conflict occurs (for example duplicate email on registration).
/// Maps to HTTP 409 Conflict.
/// </summary>
public class DuplicateEmailException : AppException
{
    public DuplicateEmailException(string? message = "Email already in use") : base(message ?? "Email already in use") { }
    public override int StatusCode => 409;
}

