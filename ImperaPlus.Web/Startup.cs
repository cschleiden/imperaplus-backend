using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Web.Providers;
using ImperaPlus.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ImperaPlus.Web
{
    public class Startup
    {
        /// <summary>
        /// Test support: Work on background threads
        /// </summary>
        public static bool StartHangfire = true;

        /// <summary>
        /// Test support: Require user confirmation
        /// </summary>
        internal static bool RequireUserConfirmation = true;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddCors();

            services.AddDbContext<ImperaContext>(options =>
            {
                string connection = "";
                options.UseSqlServer(connection);
            });

            // Auth
            services.AddIdentity<Domain.User, IdentityRole>()
                    .AddEntityFrameworkStores<ImperaContext>()
                    .AddDefaultTokenProviders();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "ImperaPlus.Web.xml");
            });            
            
            return this.RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            //app.UseFacebookAuthentication(new FacebookOptions
            //{
            //   ClientId = Configuration["Authentication:Facebook:ClientId"],
            //    AppId = Configuration["Authentication:Facebook:AppId"],
            //    AppSecret = Configuration["Authentication:Facebook:AppSecret"]                
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseIdentity();

            app.UseSignalR2();

            app.UseSwagger();
            app.UseSwaggerUi();
        }

        private IServiceProvider RegisterDependencies(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // Messaging
            if (Configuration["Environment"] == "Local")
            {
                builder.RegisterType<LocalEmailService>().AsImplementedInterfaces();
            }
            else
            {
                builder.RegisterType<MailGunEmailService>().AsImplementedInterfaces();
            }

            // Identity
            //builder.RegisterType<ApplicationUserManager>().As<UserManager<User>>().AsSelf().InstancePerRequest();
            //builder.RegisterType<UserStore<User>>().AsImplementedInterfaces();

            //builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>();
            //builder.RegisterType<RoleManager<IdentityRole>>();

            //builder.RegisterType<OopsExceptionHandler>().As<IExceptionHandler>();

            builder.RegisterType<ImperaContext>().As<DbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // Register repositories
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(GameRepository)))
            //    .Where(x => x.Name.EndsWith("Repository") && !x.IsInterface).As(x => x.GetInterfaces());

            builder.RegisterType<UserProvider>().As<IUserProvider>();

            // Register SignalR hubs
            //builder.RegisterHubs(Assembly.GetExecutingAssembly());

            // Register Domain services
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGameRepository)))
                .Where(x => x.Name.EndsWith("Service") && !x.IsInterface).As(x => x.GetInterfaces());

            // Notification
            builder.RegisterType<GamePushNotificationService>().AsImplementedInterfaces();
            builder.RegisterType<UserPushNotificationService>().AsImplementedInterfaces();

            var jsonSettings = new JsonSerializerSettings()
            {
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
                //ContractResolver = new SignalRContractResolver(),
            };
            jsonSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = false,
                AllowIntegerValues = false
            });
            //Startup.JsonSerializerSettings;
            builder.RegisterInstance(JsonSerializer.Create(jsonSettings)).As<JsonSerializer>();

            builder.RegisterModule<Application.DependencyInjectionModule>();
            builder.RegisterModule<Domain.DependencyInjectionModule>();

            //builder.RegisterType<BackgroundJobClient>().AsImplementedInterfaces();

            //if (RegisterAction != null)
            //{
            //    RegisterAction(builder);
            //}

            //builder.Register(context => hubConfiguration.Resolver
            //    .Resolve<Microsoft.AspNet.SignalR.Infrastructure.IConnectionManager>()
            //    .GetHubContext<INotificationHubContext>("notification"))
            //    .As<IHubContext<INotificationHubContext>>();

            builder.Populate(services);

            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }
    }
}
