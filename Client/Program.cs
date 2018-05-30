using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Tecla para continuar..");Console.ReadKey();
            /*
             More info: http://hamidmosalla.com/2017/10/19/policy-based-authorization-using-asp-net-core-2-and-json-web-token-jwt/
             */

            //var requestWithoutPolicyResponse = Task.Run(RequestWithClientCredentialsWithoutPolicy).Result;
            //var requestWithClientCredetials = Task.Run(RequestWithClientCredentialsWithPolicy).Result;
            //var requestWithResourceOwnerPassword = Task.Run(RequestWithResourceOwnerPasswordWithPolicy).Result;
            var requestWithResourceOwnerPasswordRoles = Task.Run(RequestWithResourceOwnerPasswordWithRole).Result;

            //Console.WriteLine($"{nameof(requestWithoutPolicyResponse)} : {requestWithoutPolicyResponse}");
            //Console.WriteLine($"{nameof(requestWithClientCredetials)} : {requestWithClientCredetials}");
            //Console.WriteLine($"{nameof(requestWithResourceOwnerPassword)} : {requestWithResourceOwnerPassword}");
            Console.WriteLine($"{nameof(requestWithResourceOwnerPasswordRoles)} : {requestWithResourceOwnerPasswordRoles}");

            Console.ReadLine();
        }

        public static async Task<string> RequestWithClientCredentialsWithoutPolicy()
        {
            async Task<string> GetAccessToken()
            {
                var openIdConnectEndPoint = await DiscoveryClient.GetAsync("http://localhost:5000");
                var tokenClient = new TokenClient(openIdConnectEndPoint.TokenEndpoint, "client1", "123654");
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

                var response = await client.GetAsync("http://localhost:5001/api/resourcewithoutpolicy");

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        public static async Task<string> RequestWithClientCredentialsWithPolicy()
        {
            async Task<string> GetAccessToken()
            {
                var openIdConnectEndPoint = await DiscoveryClient.GetAsync("http://localhost:5000");
                var tokenClient = new TokenClient(openIdConnectEndPoint.TokenEndpoint, "client1", "123654");
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

                var response = await client.GetAsync("http://localhost:5001/api/resourcewithpolicy");

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        public static async Task<string> RequestWithResourceOwnerPasswordWithPolicy()
        {
            async Task<string> GetAccessToken()
            {
                var discoveryResponse = await DiscoveryClient.GetAsync("http://localhost:5000");
                // request token
                var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "ro.client1", "123654");
                var username = Console.ReadLine();
                var password = Console.ReadLine();
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

                var response = await client.GetAsync("http://localhost:5001/api/resourcewithpolicy");

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }

        }

        public static async Task<string> RequestWithResourceOwnerPasswordWithRole()
        {
            async Task<string> GetAccessToken()
            {
                var discoveryResponse = await DiscoveryClient.GetAsync("http://localhost:5000");
                // request token
                var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "ro.client1", "123654");

                var user = Console.ReadLine();
                string username;
                string password;
                if (user == "1")
                {
                    username = "borisgr04@gmail.com";
                    password = "Atamz008.";
                }
                else {
                    username = "borisgr04@hotmail.com";
                    password = "Atamz008.";
                }
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

                var response = await client.GetAsync("http://localhost:5001/api/front");

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