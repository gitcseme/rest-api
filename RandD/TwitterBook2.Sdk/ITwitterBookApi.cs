using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitterBook2.Contracts.V1.Responses;

namespace TwitterBook2.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface ITwitterBookApi
    {
        [Get("/api/v1/posts")]
        Task<ApiResponse<List<PostResponse>>> GetAllAsync();
    }
}
