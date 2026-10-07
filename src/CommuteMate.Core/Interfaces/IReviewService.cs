using CommuteMate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponse> CreateAsync(CreateReviewRequest request,int giverID, CancellationToken ct);
    }
}
