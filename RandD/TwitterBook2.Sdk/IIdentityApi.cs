using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitterBook2.Contracts.V1.Requests;
using TwitterBook2.Contracts.V1.Responses;

namespace TwitterBook2.Sdk
{
    public interface IIdentityApi
    {
        [Post("/api/v1/identity/register")]
        Task<ApiResponse<AuthSuccessResponse>> RegisterAsync([Body] UserRegistrationRequest userRegistrationRequest);

        [Post("/api/v1/identity/login")]
        Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] UserLoginRequest loginRequest);

        [Post("/api/v1/identity/refersh")]
        Task<ApiResponse<AuthSuccessResponse>> RefreshAsync([Body] RefreshTokenRequest refreshRequest);
    }
}
