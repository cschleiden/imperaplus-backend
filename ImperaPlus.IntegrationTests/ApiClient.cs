using System.Configuration;
using System.Threading.Tasks;
using ImperaPlus.GeneratedClient;

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
        public static Task<TClientType> GetClient<TClientType>() where TClientType : ImperaHttpClient
        {
            string uri = TestSetup.TestServer.BaseAddress.ToString();
            // Remove trailing slash
            uri = uri.Substring(0, uri.Length - 1);
            var client = ImperaClientFactory.GetClient<TClientType>(uri, null, TestSetup.TestServer.CreateHandler());
            return Task.FromResult(client);
        }

        public static async Task<TClientType> GetAuthenticatedClient<TClientType>(string username, string password) where TClientType : ImperaHttpClient
        {
            // Login
            var accountClient = await GetClient<AccountClient>();
            var signinResult = await accountClient.LoginAsync("password", username, password, null, null);

            // Create requested client
            var client = await ApiClient.GetClient<TClientType>();
            client.AuthToken = signinResult.Access_token;
            return client;
        }

        public static async Task<TClientType> GetAuthenticatedClientAdminUser<TClientType>() where TClientType : ImperaHttpClient
        {
            return await GetAuthenticatedClient<TClientType>("TestAdmin", "TestAdmin");
        }

        public static async Task<TClientType> GetAuthenticatedClientDefaultUser<TClientType>() where TClientType : ImperaHttpClient
        {
            return await GetAuthenticatedClient<TClientType>((int)TestUser.Default);
        }

        public static async Task<TClientType> GetAuthenticatedClient<TClientType>(int user) where TClientType : ImperaHttpClient
        {
            return await GetAuthenticatedClient<TClientType>(
                "TestUser" + user,
                ApiClient.GetUserPassword(user));
        }

        public static string GetUserPassword(int user)
        {
            return "TestPassword" + user;
        }
    }
}