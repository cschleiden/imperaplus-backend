using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DataTables.AspNet.AspNetCore;
using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using ImperaPlus.Application;
using ImperaPlus.Application.Jobs;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Web.Filters;
using ImperaPlus.Web.Providers;
using ImperaPlus.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog.Fluent;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using StackExchange.Profiling.Storage;

namespace ImperaPlus.Web
{

    public class Startup
    {
        /// <summary>
        /// Test support: Require user confirmation
        /// </summary>
        public static bool RequireUserConfirmation = true;

        public static bool RunningUnderTest = false;

        public static ILifetimeScope Container { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                // Environment specific settings, i.e., setting db connection string. Do not create in version control repository.
                .AddJsonFile($"appsettings.environment.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (Startup.RunningUnderTest)
            {
                builder.AddJsonFile($"appsettings.test.json", optional: true);
            }

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            this.Configuration = builder.Build();
            this.Environment = env;
        }

        public IConfigurationRoot Configuration { get; }

        public IWebHostEnvironment Environment { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ImperaContext>(options =>
            {
                string connection = Configuration["DBConnection"];

                if (!Startup.RunningUnderTest)
                {
                    options.UseSqlServer(connection,
                        b => b
                            .MigrationsAssembly("ImperaPlus.Web")
                            .EnableRetryOnFailure());
                }

                options.UseLazyLoadingProxies();

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
            services
                .AddIdentity<Domain.User, IdentityRole>(options =>
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

                    options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                    options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                    options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                })
                .AddEntityFrameworkStores<ImperaContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "bearer_token";
                // options.CookieManager
                // Ensure that we never redirect when user is not authorized, but only return 401 response
                // options.Cookies.ApplicationCookie.AutomaticChallenge = false;
                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
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
            });

            services
                .AddOpenIddict(options =>
                {
                    options.AddCore(x =>
                    {
                        x.UseEntityFrameworkCore(c => c.UseDbContext<ImperaContext>());
                    });

                    options.AddServer(c =>
                    {
                        if (this.Environment.IsDevelopment())
                        {
                            c.DisableHttpsRequirement();
                            c.AddEphemeralSigningKey();
                        }

                        c.AllowPasswordFlow();
                        c.AllowRefreshTokenFlow();
                        c.EnableTokenEndpoint("/Account/Token");

                        c.RegisterScopes(OpenIddictConstants.Scopes.Roles);

                        // Don't require client_id
                        c.AcceptAnonymousClients();

                        c.UseMvc();
                    });

                    options.AddValidation(builder =>
                    {
                        builder.AddEventHandler<OpenIddictValidationEvents.RetrieveToken>(
                            notification =>
                            {
                                notification.Context.Token = notification.Context.Request.Cookies["bearer_token"];

                                if (string.IsNullOrEmpty(notification.Context.Token))
                                {
                                    notification.Context.Token = notification.Context.Request.Query["access_token"];
                                }

                                return Task.FromResult(OpenIddictValidationEventState.Handled);
                            });
                    });
                });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });

            services
                .AddControllersWithViews(config =>
                {
                    config.Filters.Add(new CheckModelForNull());
                    config.Filters.Add(typeof(ApiExceptionFilterAttribute));
                })
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    opt.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        // Do not use camel case for enums
                        NamingStrategy = new DefaultNamingStrategy(),
                        AllowIntegerValues = true
                    });
                });

            // Miniprofiler
            services
               .AddMiniProfiler(config =>
               {
                   (config.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(5);
                   config.RouteBasePath = "/admin/profiler";
               })
               .AddEntityFramework();

            //services.AddMemoryCache();

            // DataTables for the admin interface
            services.RegisterDataTables();

            // Hangfire
            services.AddHangfire(x =>
            {
                x.UseNLogLogProvider()
                 .UseFilter(new JobExpirationTimeAttribute())
                 .UseConsole();

                if (Startup.RunningUnderTest)
                {
                    x.UseMemoryStorage();
                }
                else
                {
                    x.UseSqlServerStorage(Configuration["DBConnection"]);
                }

                x.UseSerializerSettings(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            });
            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { JobQueues.Critical, JobQueues.Normal };
                options.WorkerCount = 4;
                options.SchedulePollingInterval = TimeSpan.FromSeconds(30);
                options.ServerCheckInterval = TimeSpan.FromSeconds(60);
                options.HeartbeatInterval = TimeSpan.FromSeconds(60);
            });

            services.AddSingleton(_ => new JsonSerializer
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            });

            services
                .AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                })
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.PayloadSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.PayloadSerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                });

            // Have SignalR use the name claim as user id
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            // Swagger document
            services.AddOpenApiDocument();

            // Allow other parts to access the http context
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register AutoMapper
            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            var assembliesTypes = assemblyNames
                .Where(a => a.Name.Contains("ImperaPlus", StringComparison.OrdinalIgnoreCase))
                .SelectMany(an => Assembly.Load(an).GetTypes())
                .Where(p => typeof(Profile).IsAssignableFrom(p) && p.IsPublic && !p.IsAbstract)
                .Distinct()
                .ToArray();

            services.AddAutoMapper(assembliesTypes);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            ImperaContext dbContext,
            DbSeed dbSeed)
        {
            if (RunningUnderTest)
            {
                Container = app.ApplicationServices.GetAutofacRoot();
            }

            if (env.IsDevelopment())
            {
                app.UsePathBase("/api");
            }

            NLog.LogManager.Configuration.Variables["configDir"] = Configuration["LogDir"];

#if DEBUG
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
#endif

            // Enable Cors
            app.UseCors(b => b
                .WithOrigins("http://localhost:8080", "https://dev.imperaonline.de", "https://imperaonline.de", "https://www.imperaonline.de")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-MiniProfiler-Ids")
                .AllowCredentials());

            app.UseStaticFiles();

            // Add profiler support
            app.UseMiniProfiler();

            // Configure swagger generation & UI
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            // Auth
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapAreaControllerRoute(
                    "admin",
                    "admin",
                    "Admin/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    "default", "{controller=Home}/{action=Index}/{id?}");


                endpoints.MapHub<Hubs.MessagingHub>("/signalr/chat");
                endpoints.MapHub<Hubs.GameHub>("/signalr/game");
            });

            // Initialize database
            Log.Info("Initializing database...").Write();
            if (env.IsDevelopment())
            {
                if (Startup.RunningUnderTest)
                {
                    dbContext.Database.EnsureDeleted();
                }
                else
                {
                    dbContext.Database.Migrate();
                }
            }
            else
            {
                Log.Info("Starting migration...").Write();
                dbContext.Database.Migrate();
                Log.Info("...done.").Write();
            }
            Log.Info("...done.").Write();

            Log.Info("Seeding database...").Write();
            dbSeed.Seed(dbContext).Wait();
            Log.Info("...done.").Write();

            // Hangfire
            app.UseHangfireDashboard("/Admin/Hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            // Configure Impera background jobs
            JobConfig.Configure();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
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

            // Ensure that we can override it from a test
            builder.RegisterType<UserProvider>().As<IUserProvider>().PreserveExistingDefaults();

            // Register Domain services
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGameRepository)))
                .Where(x => x.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase) && !x.IsInterface).As(x => x.GetInterfaces());

            // Notification
            builder.RegisterType<GamePushNotificationService>().AsImplementedInterfaces();
            builder.RegisterType<UserPushNotificationService>().AsImplementedInterfaces();

            var jsonSettings = new JsonSerializerSettings()
            {
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat
            };
            jsonSettings.Converters.Add(new StringEnumConverter
            {
                NamingStrategy = new DefaultNamingStrategy(),
                AllowIntegerValues = false
            });

            builder.RegisterInstance(JsonSerializer.Create(jsonSettings)).As<JsonSerializer>();

            builder.RegisterModule<Application.DependencyInjectionModule>();
            builder.RegisterModule<Domain.DependencyInjectionModule>();

            builder.RegisterType<BackgroundJobClient>().AsImplementedInterfaces();
        }
    }
}
