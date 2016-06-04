using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Application.Tests
{
    [TestClass]
    public class TestSetup : TestSupport.TestSetup
    {
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);

            builder.RegisterType<Application.MapTemplateService>().AsImplementedInterfaces();
            builder.RegisterType<Application.Games.GameService>().AsImplementedInterfaces();
            builder.RegisterType<Application.Play.PlayService>().AsImplementedInterfaces();

            builder.RegisterType<Domain.Services.GameService>().AsImplementedInterfaces();

            builder.RegisterType<ImperaPlus.Application.Visibility.VisibilityModifierFactory>().AsImplementedInterfaces();
        }

        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            new TestSetup().TestInit(context);

            AutoMapperConfig.Configure();
        }
    }
}
