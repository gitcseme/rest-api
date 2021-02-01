using Refit;
using System;
using System.Threading.Tasks;
using TwitterBook2.Contracts.V1.Requests;
using TwitterBook2.Sdk;

namespace TweetBook2.Sdk.Sample
{
    class Program
    {
        /*
         * Refit can do everything with self auto generated code.
         */

        static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
            var twitterBookApi = RestService.For<ITwitterBookApi>("https://localhost:5001", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Shuvo$123"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Shuvo$123"
            });

            cachedToken = loginResponse.Content.Token;

            var allPosts = await twitterBookApi.GetAllAsync();
        }
    }
}
