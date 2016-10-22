using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.SignalR;
using Hangfire;
using ImperaPlus.Application;
using ImperaPlus.Application.Jobs;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Web.Hubs;
using ImperaPlus.Web.Providers;
using ImperaPlus.Web.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

        public HubConfiguration HubConfiguration { get; set; } = new HubConfiguration();

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

            this.Configuration = builder.Build();
            this.Environment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ImperaContext>(options =>
            {
                string connection = Configuration["DBConnection"];
                options.UseSqlServer(connection, b=> b.MigrationsAssembly("ImperaPlus.Web"));
            });

            // Auth
            services.AddIdentity<Domain.User, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    if (this.Environment.IsDevelopment())
                    {
                        options.SignIn.RequireConfirmedEmail = false;
                    }
                })
                .AddEntityFrameworkStores<ImperaContext>()
                .AddDefaultTokenProviders();

            var openIddict = services.AddOpenIddict<ImperaContext>()
                .EnableTokenEndpoint("/api/Account/Token")
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow();

            if (this.Environment.IsDevelopment())
            {
                openIddict
                    // During development, you can disable the HTTPS requirement.
                    .DisableHttpsRequirement()
                    .AddEphemeralSigningKey();
            }

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "ImperaPlus.Web.xml");
            });

            services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = false,
                        AllowIntegerValues = false
                    });
                });
            services.AddCors();

            // Hangire
            services.AddHangfire(x =>
                x.UseNLogLogProvider()
                 .UseFilter(new JobExpirationTimeAttribute())
                 .UseSqlServerStorage(Configuration["DBConnection"]));

            return this.RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            ImperaContext dbContext,
            DbSeed dbSeed)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            
            // Enable Cors
            app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Auth
            app.UseIdentity();
            //app.UseFacebookAuthentication(new FacebookOptions
            //{
            //   ClientId = Configuration["Authentication:Facebook:ClientId"],
            //    AppId = Configuration["Authentication:Facebook:AppId"],
            //    AppSecret = Configuration["Authentication:Facebook:AppSecret"]                
            //});            
            app.UseOAuthValidation();
            app.UseOpenIddict();

            app.UseMvc();
            

            app.UseSignalR2(HubConfiguration);

            app.UseSwagger();
            app.UseSwaggerUi();

            // Initialize database
            if (env.IsDevelopment())
            {
                // Always recreate in development
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            else
            {
                dbContext.Database.Migrate();
            }
                
            dbSeed.Seed(dbContext).Wait();

            AutoMapperConfig.Configure();

            // Hangfire
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] { JobQueues.Critical, JobQueues.Normal },                
            });
            app.UseHangfireDashboard();

            Hangfire.Common.JobHelper.SetSerializerSettings(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            // Configure Impera background jobs
            JobConfig.Configure();
        }

        private IServiceProvider RegisterDependencies(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var builder = new ContainerBuilder();

            // Messaging
            if (Environment.IsDevelopment())
            {
                builder.RegisterType<LocalEmailService>().AsImplementedInterfaces();
            }
            else
            {
                builder.RegisterType<MailGunEmailService>().AsImplementedInterfaces();
            }

            //builder.RegisterType<OopsExceptionHandler>().As<IExceptionHandler>();

            builder.RegisterType<ImperaContext>().As<DbContext>().AsSelf(); //.InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterType<DbSeed>().AsSelf();

            // Register repositories
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(GameRepository)))
            //    .Where(x => x.Name.EndsWith("Repository") && !x.IsInterface).As(x => x.GetInterfaces());

            builder.RegisterType<UserProvider>().As<IUserProvider>();

            // Register SignalR hubs
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

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

            builder.RegisterInstance(JsonSerializer.Create(jsonSettings)).As<JsonSerializer>();

            builder.RegisterModule<Application.DependencyInjectionModule>();
            builder.RegisterModule<Domain.DependencyInjectionModule>();

            builder.RegisterType<Hangfire.BackgroundJobClient>().AsImplementedInterfaces();

            //if (RegisterAction != null)
            //{
            //    RegisterAction(builder);
            //}

            IContainer container = null;

            builder.Register(context => this.HubConfiguration.Resolver
               .Resolve<Microsoft.AspNet.SignalR.Infrastructure.IConnectionManager>()
               .GetHubContext<INotificationHubContext>("notification"))
               .As<IHubContext<INotificationHubContext>>();

            builder.Populate(services);

            container = builder.Build();

            Domain.DomainDepsResolver.ScopeGen = () =>
            {
                var activator = container.Resolve<IHttpContextAccessor>();
                return activator.HttpContext.RequestServices.GetService<ILifetimeScope>();
            };
            this.HubConfiguration.Resolver = new AutofacDependencyResolver(container);

            return container.Resolve<IServiceProvider>();
        }
    }
}
