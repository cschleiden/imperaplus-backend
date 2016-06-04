using System;
using Autofac;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Services;
using ImperaPlus.TestSupport.Testdata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.TestSupport
{
    public class TestBase
    {
        protected ImperaContext Context;
        protected UnitOfWork UnitOfWork;
        protected TestData TestData;
        protected User TestUser;
        protected User BotUser;

        [TestInitialize]
        public virtual void TestInit()
        {
            this.SetupScope();
            this.TestData = new TestData(this.Context, this.Scope, new GameService(this.Scope, this.UnitOfWork));

            // Ensure a user does exist
            this.TestUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "TestUser"
            };
            this.Context.Users.Add(this.TestUser);

            this.BotUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Bot"
            };
            this.Context.Users.Add(this.BotUser);
            
            this.Context.SaveChanges();

            TestUserProvider.User = this.TestUser;
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            this.DisposeScope();
        }

        protected void SetupScope()
        {
            this.Scope = TestSetup.Container.BeginLifetimeScope("AutofacWebRequest");
            this.Context = this.Scope.Resolve<ImperaContext>();
            this.UnitOfWork = new UnitOfWork(this.Context);
        }

        protected void DisposeScope()
        {
            this.Scope.Dispose();
        }

        public ILifetimeScope Scope { get; private set; }
    }
}