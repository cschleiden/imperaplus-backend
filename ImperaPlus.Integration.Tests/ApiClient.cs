using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace ImperaPlus.Integration.Tests
{
    public enum TestUser
    {
        Default = 0,
        User1 = 1,
        User2 = 2,
        User3 = 3
    }

    public class ApiClient
    {
        public static Task<HttpClient> GetClient()
        {
            var client = new HttpClient(TestSetup.TestServer.Handler)
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseUri"])
            };

            return Task.FromResult(client);
        }

        public static async Task<HttpClient> GetAuthenticatedClientAdminUser()
        {
            return await GetAuthenticatedClient("TestAdmin", "TestAdmin");
        }

        public static async Task<HttpClient> GetAuthenticatedClientDefaultUser()
        {
            return await GetAuthenticatedClient((int)TestUser.Default);
        }

        public static async Task<HttpClient> GetAuthenticatedClient(int user)
        {
            return await GetAuthenticatedClient(
                ConfigurationManager.AppSettings["TestUser" + user], 
                ConfigurationManager.AppSettings["TestPassword" + user]);
        }

        public static async Task<HttpClient> GetAuthenticatedClient(string username, string password)
        {
            var client = await GetClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var result = await client.PostAsync("Token", content);
            var tokenString = await result.Content.ReadAsStringAsync();

            if (tokenString.Contains("incorrect"))
            {
                Assert.Fail("Cannot authenticate, user does not exist?");
            }

            if (tokenString.Contains("confirmed"))
            {
                Assert.Fail("Cannot authenticate, user is not confirmed");
            }

            var ticket = JObject.Parse(tokenString);
            var token = ticket["access_token"].ToString();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}