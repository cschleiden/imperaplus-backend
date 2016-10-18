using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ImperaPlus.Application;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.TestSupport.Testdata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            AutoMapperConfig.Configure();

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
            var builder = new ContainerBuilder();

            var serviceCollection = new ServiceCollection()
                 .AddEntityFrameworkInMemoryDatabase();

            var dbOptionsBuilder = new DbContextOptionsBuilder<ImperaContext>();

            builder.Register(_ => dbOptionsBuilder.Options).As<DbContextOptions<ImperaContext>>();

            this.RegisterDependencies(builder);

            builder.Populate(serviceCollection);

            this.Container = builder.Build();

            dbOptionsBuilder.UseInMemoryDatabase().UseInternalServiceProvider(new AutofacServiceProvider(this.Container));            

            this.Scope = Container.BeginLifetimeScope("AutofacWebRequest");
            this.Context = this.Scope.Resolve<ImperaContext>();
            this.UnitOfWork = new UnitOfWork(this.Context);
        }

        protected void DisposeScope()
        {
            this.Scope.Dispose();
        }

        public ILifetimeScope Scope { get; private set; }

        public IContainer Container;

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ImperaContext>().As<IImperaContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TestUserProvider>().As<IUserProvider>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<MapTemplateProvider>().As<IMapTemplateProvider>();
            builder.RegisterType<AttackService>().As<IAttackService>();
            builder.RegisterModule(new Domain.DependencyInjectionModule());

            builder.RegisterType<Application.MapTemplateService>().AsImplementedInterfaces();
            builder.RegisterType<Application.Games.GameService>().AsImplementedInterfaces();
            builder.RegisterType<Application.Play.PlayService>().AsImplementedInterfaces();
            builder.RegisterType<Domain.Services.GameService>().AsImplementedInterfaces();
            builder.RegisterType<ImperaPlus.Application.Visibility.VisibilityModifierFactory>().AsImplementedInterfaces();
        }
    }
}