using Autofac;
using ImperaPlus.Application;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.TestSupport
{
    public class TestSetup
    {
        public static IContainer Container;

        public void TestInit(TestContext testContext)
        {
            // Configure in-memory db
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();

            var builder = new ContainerBuilder();

            this.RegisterDependencies(builder);
            
            Container = builder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<TestDbConnectionFactory>().AsImplementedInterfaces();

            builder.RegisterType<ImperaContext>().As<IImperaContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TestUserProvider>().As<IUserProvider>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<MapTemplateProvider>().As<IMapTemplateProvider>();

            builder.RegisterType<AttackService>().As<IAttackService>();

            builder.RegisterModule(new Domain.DependencyInjectionModule());
        }
    }
}
