using System;
using System.Threading.Tasks;
using ImperaPlus.GeneratedClient;

namespace ImperaPlus.IntegrationTests
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
        public static Task<TClientType> GetClient<TClientType>() where TClientType : ImperaHttpClient
        {
            var uri = TestSetup.TestServer.BaseAddress.ToString();
            // Remove trailing slash
            uri = uri.Substring(0, uri.Length - 1);
            var client = ImperaClientFactory.GetClient<TClientType>(uri, null, TestSetup.TestServer.CreateHandler());
            return Task.FromResult(client);
        }

        public static async Task<TClientType> GetAuthenticatedClient<TClientType>(string username, string password)
            where TClientType : ImperaHttpClient
        {
            // Login
            try
            {
                var accountClient = await GetClient<AccountClient>();
                var signinResult = await accountClient.LoginAsync("password", username, password, null, null);

                // Create requested client
                var client = await GetClient<TClientType>();
                client.AuthToken = signinResult.Access_token;
                return client;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<TClientType> GetAuthenticatedClientAdminUser<TClientType>()
            where TClientType : ImperaHttpClient
        {
            return await GetAuthenticatedClient<TClientType>("TestAdmin", "TestAdmin");
        }

        public static async Task<TClientType> GetAuthenticatedClientDefaultUser<TClientType>()
            where TClientType : ImperaHttpClient
        {
            return await GetAuthenticatedClient<TClientType>((int)TestUser.Default);
        }

        public static async Task<TClientType> GetAuthenticatedClient<TClientType>(int user)
            where TClientType : ImperaHttpClient
        {
            return await GetAuthenticatedClient<TClientType>(
                "TestUser" + user,
                GetUserPassword(user));
        }

        public static string GetUserPassword(int user)
        {
            return "TestPassword" + user;
        }
    }
}
