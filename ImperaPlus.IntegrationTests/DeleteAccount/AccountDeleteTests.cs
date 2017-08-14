using System.Threading;
using System.Threading.Tasks;
using ImperaPlus.DTO.Account;
using ImperaPlus.GeneratedClient;
using ImperaPlus.Integration.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.IntegrationTests.DeleteAccount
{
    [TestClass]
    public class AccountDeleteTests : BaseIntegrationTest
    {
        private AccountClient defaultAccountClient;
        private UserInfo defaultUser;

        private AccountClient otherAccountClient;
        private UserInfo otherUser;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.defaultAccountClient = ApiClient.GetAuthenticatedClientDefaultUser<AccountClient>().Result;
            this.otherAccountClient = ApiClient.GetAuthenticatedClient<AccountClient>(1).Result;

            this.defaultUser = this.defaultAccountClient.GetUserInfoAsync().Result;
            this.otherUser = this.otherAccountClient.GetUserInfoAsync().Result;
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod]
        public async Task DeleteAcccount()
        {
            // Arrange
            await this.SetupAccount();

            // Act
            await this.defaultAccountClient.DeleteAccountAsync(new DeleteAccountBindingModel
            {
                Password = ApiClient.GetUserPassword(0)
            });

            // Force clean up jo
            this.Log("Force job...");
            Hangfire.RecurringJob.Trigger(Application.Jobs.UserCleanupJob.JobId);

            await Task.Delay(10000);
            this.Log("done.");
        }

        private async Task SetupAccount()
        {
            await SendMessages();
        }

        private async Task SendMessages()
        {
            var messageClient = await ApiClient.GetAuthenticatedClientDefaultUser<MessageClient>();

            // Send message to other user
            await messageClient.PostSendAsync(new DTO.Messages.SendMessage
            {
                To = new DTO.Users.UserReference
                {
                    Id = this.otherUser.UserId,
                    Name = this.otherUser.UserName
                },

                Subject = "Test",

                Text = "Test"
            });

            // Send message to self
            await messageClient.PostSendAsync(new DTO.Messages.SendMessage
            {
                To = new DTO.Users.UserReference
                {
                    Id = this.defaultUser.UserId,
                    Name = this.defaultUser.UserName
                },

                Subject = "Test",

                Text = "Test"
            });
        }
    }
}
