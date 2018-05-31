using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsumingApiRest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var aaaqa = RequestWithResourceOwnerPasswordWithRole("http://localhost:53469/api/anibal", "anibal@anibal.com", "1.Admin.1");
            var aaaqa = RequestWithClientCredentialsWithPolicy("http://localhost:53469/api/anibal");
            var requestWithResourceOwnerPasswordRoles = aaaqa.Result;

            Console.WriteLine($"{nameof(requestWithResourceOwnerPasswordRoles)} : {requestWithResourceOwnerPasswordRoles}");

            Console.ReadLine();
        }

        public static async Task<string> RequestWithClientCredentialsWithPolicy(string url)
        {
            async Task<string> GetAccessToken()
            {
                var openIdConnectEndPoint = await DiscoveryClient.GetAsync("http://localhost:5000");
                var tokenClient = new TokenClient(openIdConnectEndPoint.TokenEndpoint, "ClienteAnibal", "123654");
                var accessToken = await tokenClient.RequestClientCredentialsAsync("Api1");

                if (accessToken.IsError)
                {
                    Console.WriteLine(accessToken.Error);
                    return accessToken.Error;
                }

                Console.WriteLine(accessToken.Json);

                return accessToken.AccessToken;
            }

            using (var client = new HttpClient())
            {
                var accessToken = await GetAccessToken();

                client.SetBearerToken(accessToken);

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        public static async Task<string> RequestWithResourceOwnerPasswordWithRole(string url, string username, string password)
        {
            async Task<string> GetAccessToken()
            {
                var discoveryResponse = await DiscoveryClient.GetAsync("http://localhost:5000");
                // request token
                var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "ClienteAnibal", "123654");

                var accessToken = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, "Api1");


                if (accessToken.IsError)
                {
                    Console.WriteLine(accessToken.Error);
                    return accessToken.Error;
                }

                Console.WriteLine(accessToken.Json);

                return accessToken.AccessToken;
            }

            using (var client = new HttpClient())
            {
                var accessToken = await GetAccessToken();

                client.SetBearerToken(accessToken);

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }

        }
    }
}
