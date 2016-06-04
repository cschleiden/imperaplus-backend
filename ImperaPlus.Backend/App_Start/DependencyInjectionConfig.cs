using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using ImperaPlus.Backend.Hubs;
using ImperaPlus.Backend.Providers;
using ImperaPlus.DataAccess;
using ImperaPlus.DataAccess.Repositories;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using Hangfire;
using ImperaPlus.Backend.Identity;
using Owin;
using ImperaPlus.Domain;
using System.Configuration;
using ImperaPlus.Backend.Services;
using Autofac.Integration.Mvc;

namespace ImperaPlus.Backend.App_Start
{
    public static class DependencyInjectionConfig
    {
        public const string RequestLifetimeScopeName = "AutofacWebRequest";

        public static IContainer Container { get; private set; }

        public static Action<ContainerBuilder> RegisterAction { get; set; }

        public static void Init(IAppBuilder app, HttpConfiguration httpConfiguration, HubConfiguration hubConfiguration, bool setupMvc)
        {
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Messaging
            if (ConfigurationManager.AppSettings["Environment"] == "Local")
            {                
                builder.RegisterType<LocalEmailService>().AsImplementedInterfaces();
            }
            else
            {
                builder.RegisterType<MailGunEmailService>().AsImplementedInterfaces();
            }

            builder.RegisterType<IdentityMessageService>().AsImplementedInterfaces();

            // Identity
            builder.RegisterType<ApplicationUserManager>().As<UserManager<User>>().AsSelf().InstancePerRequest();
            builder.RegisterType<UserStore<User>>().AsImplementedInterfaces();

            builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>();
            builder.RegisterType<RoleManager<IdentityRole>>();

            // Register the Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            if (setupMvc)
            {
                // Register MVC controllers
                builder.RegisterControllers(Assembly.GetExecutingAssembly());
            }

            builder.RegisterType<OopsExceptionHandler>().As<IExceptionHandler>();

            builder.RegisterType<ImperaContext>().As<DbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            
            // Register repositories
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof (GameRepository)))
                .Where(x => x.Name.EndsWith("Repository") && !x.IsInterface).As(x => x.GetInterfaces());

            builder.RegisterType<UserProvider>().As<IUserProvider>();
            
            // Register SignalR hubs
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            // Register Domain services
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof (IGameRepository)))
                .Where(x => x.Name.EndsWith("Service") && !x.IsInterface).As(x => x.GetInterfaces());

            // Notification
            builder.RegisterType<GamePushNotificationService>().AsImplementedInterfaces();
            builder.RegisterType<UserPushNotificationService>().AsImplementedInterfaces();

            var jsonSettings = Startup.JsonSerializerSettings;
            
            builder.RegisterInstance(JsonSerializer.Create(jsonSettings)).As<JsonSerializer>();

            builder.RegisterModule<Application.DependencyInjectionModule>();
            builder.RegisterModule<Domain.DependencyInjectionModule>();

            builder.RegisterType<BackgroundJobClient>().AsImplementedInterfaces();

            if (RegisterAction != null)
            {
                RegisterAction(builder);
            }

            builder.Register(context => hubConfiguration.Resolver
                .Resolve<Microsoft.AspNet.SignalR.Infrastructure.IConnectionManager>()
                .GetHubContext<INotificationHubContext>("notification"))
                .As<IHubContext<INotificationHubContext>>();

            var container = builder.Build();

            // Configure Web API with the dependency resolver.
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Configure SignalR DI
            hubConfiguration.Resolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(httpConfiguration);

            // Setup MVC
            if (setupMvc)
            {
                System.Web.Mvc.DependencyResolver.SetResolver(
                    new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
                app.UseAutofacMvc();
            }

            Container = container;
        }
    }
}