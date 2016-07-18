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
            var client = ImperaClientFactory.GetClient<TClientType>(ConfigurationManager.AppSettings["BaseUri"], null, TestSetup.TestServer.Handler);
            return Task.FromResult(client);
        }

        public static async Task<TClientType> GetAuthenticatedClient<TClientType>(string username, string password) where TClientType : ImperaHttpClient
        {
            // Login
            var accountClient = await GetClient<AccountClient>();
            var token = await accountClient.LoginAsync(username, password);

            // Create requested client
            var client = await ApiClient.GetClient<TClientType>();
            client.AuthToken = token;
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
                ConfigurationManager.AppSettings["TestUser" + user],
                ConfigurationManager.AppSettings["TestPassword" + user]);
        }
    }
}