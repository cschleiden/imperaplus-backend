using AspNet.Security.OpenIdConnect.Primitives;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataTables.AspNet.AspNetCore;
using Hangfire;
using Hangfire.Dashboard;
using ImperaPlus.Application;
using ImperaPlus.Application.Jobs;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Web.Filters;
using ImperaPlus.Web.Providers;
using ImperaPlus.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog.Extensions.Logging;
using NLog.Web;
using StackExchange.Profiling.Storage;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ImperaPlus.Web
{

    public class Startup
    {
        /// <summary>
        /// Test support: Require user confirmation
        /// </summary>
        internal static bool RequireUserConfirmation = true;

        public Startup(IHostingEnvironment env)
        {
            env.ConfigureNLog("nlog.config");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                // Environment specific settings, i.e., setting db connecting string. Do not create in version control repository.
                .AddJsonFile($"appsettings.environment.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
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

                options.UseSqlServer(connection, b => b.MigrationsAssembly("ImperaPlus.Web"));

                options.UseOpenIddict();
            });

            services.AddCors(opts =>
            {
                var policy = new CorsPolicy()
                {
                    SupportsCredentials = false
                };

                policy.ExposedHeaders.Add("X-MiniProfiler-Ids");
                policy.Headers.Add("X-MiniProfiler-Ids");

                opts.AddPolicy(
                    opts.DefaultPolicyName,
                    policy);
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

                    // Ensure that we never redirect when user is not authorized, but only return 401 response
                    options.Cookies.ApplicationCookie.AutomaticAuthenticate = false;
                    options.Cookies.ApplicationCookie.AutomaticChallenge = false;
                    options.Cookies.ApplicationCookie.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = ctx =>
                        {
                            ctx.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                            return Task.CompletedTask;
                        },

                        OnValidatePrincipal = ctx =>
                        {
                            return Task.CompletedTask;
                        }
                    };

                    options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                    options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                    options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                })
                .AddEntityFrameworkStores<ImperaContext>()
                .AddDefaultTokenProviders();

            services
                .AddOpenIddict(options =>
                {
                    options.AddEntityFrameworkCoreStores<ImperaContext>();

                    options.EnableTokenEndpoint("/api/Account/Token");

                    options.AddMvcBinders();

                    options.AllowPasswordFlow();

                    options.AllowRefreshTokenFlow();

                    if (this.Environment.IsDevelopment())
                    {
                        // During development, you can disable the HTTPS requirement.
                        options.DisableHttpsRequirement();

                        options.AddEphemeralSigningKey();
                    }
                });

            // Swagger
            //services.AddTransient<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<FormFilter>();
                options.DescribeStringEnumsInCamelCase();
                options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImperaPlus.Web.xml"));
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMvc(config =>
                {
                    config.Filters.Add(new CheckModelForNull());
                    config.Filters.Add(typeof(ApiExceptionFilterAttribute));
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = false,
                        AllowIntegerValues = true
                    });
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services
                .AddMiniProfiler()
                .AddEntityFramework();

            services.AddMemoryCache();

            // DataTables for the admin interface
            services.RegisterDataTables();

            // Set default authorization policy
            services.AddAuthorization(o =>
            {
                var builder = new AuthorizationPolicyBuilder();
                builder.AuthenticationSchemes.Add(AspNet.Security.OAuth.Validation.OAuthValidationDefaults.AuthenticationScheme);
                builder.RequireAuthenticatedUser();
                o.DefaultPolicy = builder.Build();
            });

            // Hangire
            services.AddHangfire(x =>
                x.UseNLogLogProvider()
                 .UseFilter(new JobExpirationTimeAttribute())
                 .UseSqlServerStorage(Configuration["DBConnection"]));

            services.AddSingleton(_ => new JsonSerializer
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new SignalRContractResolver()
            });

            services.AddSignalR(options => options.Hubs.EnableDetailedErrors = true);

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
            loggerFactory.AddNLog();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.AddNLogWeb();
            NLog.LogManager.Configuration.Variables["configDir"] = Configuration["LogDir"];

            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            // Enable Cors
            app.UseCors(b => b
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-MiniProfiler-Ids")
                .DisallowCredentials()
                .Build());

            // Auth
            app.UseIdentity();

            /*app.UseFacebookAuthentication(new FacebookOptions
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            });

            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions
            {
                ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"],
                ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"]
            });*/

            app.UseOAuthValidation(options =>
            {
                options.Events = new AspNet.Security.OAuth.Validation.OAuthValidationEvents
                {
                    // Note: for SignalR connections, the default Authorization header does not work,
                    // because the WebSockets JS API doesn't allow setting custom parameters.
                    // To work around this limitation, the access token is retrieved from the query string.
                    OnRetrieveToken = context =>
                    {
                        context.Token = context.Request.Query["bearer_token"];

                        if (string.IsNullOrEmpty(context.Token))
                        {
                            context.Token = context.Request.Cookies["bearer_token"];
                        }

                        return Task.FromResult(0);
                    }
                };
            });
            app.UseOpenIddict();

            // Enable serving client and static assets
            app.UseResponseCompression();
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Do not cache main entry point.
                    if (!ctx.File.Name.Contains("index.html"))
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=6000000");
                    }
                }
            });

            app.UseMiniProfiler(new StackExchange.Profiling.MiniProfilerOptions
            {
                RouteBasePath = "~/admin/profiler",

                SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter(),

                // Control storage
                Storage = new MemoryCacheStorage(TimeSpan.FromMinutes(60))
            });

            app.UseMvc(routes =>
            {
                // Route for sub areas, i.e. Admin
                routes.MapRoute("areaRoute", "{area:exists}/{controller=News}/{action=Index}");

                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWebSockets();
            app.UseSignalR();

            app.UseSwagger();
            app.UseSwaggerUi();

            // Initialize database
            if (env.IsDevelopment())
            {
                // Always recreate in development
                //dbContext.Database.EnsureDeleted();
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
                WorkerCount = 2,
                SchedulePollingInterval = TimeSpan.FromSeconds(30),
                ServerCheckInterval = TimeSpan.FromSeconds(60),
                HeartbeatInterval = TimeSpan.FromSeconds(60)
            });
            app.UseHangfireDashboard("/Admin/Hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

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
                builder.RegisterInstance(new MailGunSettings
                {
                    ApiKey = Configuration["MailGunApiKey"],
                    Domain = Configuration["MailGunDomain"]
                });
                builder.RegisterType<MailGunEmailService>().AsImplementedInterfaces();
            }

            //builder.RegisterType<OopsExceptionHandler>().As<IExceptionHandler>();

            builder.RegisterType<ImperaContext>().As<DbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<DbSeed>().AsSelf();

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
                ContractResolver = new SignalRContractResolver()
            };
            jsonSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = false,
                AllowIntegerValues = false
            });

            builder.RegisterInstance(JsonSerializer.Create(jsonSettings)).As<JsonSerializer>();

            builder.RegisterModule<Application.DependencyInjectionModule>();
            builder.RegisterModule<Domain.DependencyInjectionModule>();

            builder.RegisterType<BackgroundJobClient>().AsImplementedInterfaces();


            builder.Populate(services);

            IContainer container = null;
            container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }
    }
}
