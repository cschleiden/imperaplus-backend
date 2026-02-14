using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DataTables.AspNet.AspNetCore;
using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using ImperaPlus.Application;
using ImperaPlus.Application.Jobs;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Web.Filters;
using ImperaPlus.Web.Hubs;
using ImperaPlus.Web.Providers;
using ImperaPlus.Web.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
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
using NLog;
using NLog.Fluent;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using OpenIddict.Validation.AspNetCore;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using DependencyInjectionModule = ImperaPlus.Application.DependencyInjectionModule;

namespace ImperaPlus.Web;

public class Startup
{
    /// <summary>
    ///     Test support: Require user confirmation
    /// </summary>
    public static bool RequireUserConfirmation = true;

    public static bool RunningUnderTest = false;

    public Startup(IWebHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            // Environment specific settings, i.e., setting db connection string. Do not create in version control repository.
            .AddJsonFile("appsettings.environment.json", true)
            .AddJsonFile($"/run/secrets/appsettings.{env.EnvironmentName}.json", true)
            .AddEnvironmentVariables();

        if (RunningUnderTest)
        {
            builder.AddJsonFile("appsettings.test.json", true);
        }

        if (env.IsDevelopment())
        {
            builder.AddUserSecrets<Startup>(true);
        }

        Configuration = builder.Build();
        Environment = env;
    }

    public static ILifetimeScope Container { get; private set; }

    public IConfigurationRoot Configuration { get; }

    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        Log.Info().Message("DB: " + Configuration["DBConnection"]).Write();

        services.AddDbContext<ImperaContext>(options =>
        {
            var connection = Configuration["DBConnection"];

            if (!RunningUnderTest)
            {
                options.UseSqlServer(connection,
                    b => b
                        .MigrationsAssembly("ImperaPlus.Web")
                        .EnableRetryOnFailure());
            }
            else
            {
                options.UseInMemoryDatabase("ImperaPlusTest");
            }

            options.UseLazyLoadingProxies(true);
            options.UseChangeTrackingProxies(false);

            options.UseOpenIddict();
        });

        services.AddCors(opts =>
        {
            var policy = new CorsPolicy { SupportsCredentials = false };

            policy.ExposedHeaders.Add("X-MiniProfiler-Ids");
            policy.Headers.Add("X-MiniProfiler-Ids");

            opts.AddPolicy(
                opts.DefaultPolicyName,
                policy);
        });

        // Auth
        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ImperaContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;

            options.User.RequireUniqueEmail = true;

            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;

            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            if (Environment.IsDevelopment())
            {
                options.SignIn.RequireConfirmedEmail = false;
            }
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "bearer_token";
            // options.CookieManager
            // Ensure that we never redirect when user is not authorized, but only return 401 response
            // options.Cookies.ApplicationCookie.AutomaticChallenge = false;
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                },
                OnValidatePrincipal = ctx => Task.CompletedTask
            };
        });

        services
            .AddOpenIddict()
            .AddCore(x =>
            {
                x.UseEntityFrameworkCore().UseDbContext<ImperaContext>();
            })
            .AddServer(c =>
            {
                c.SetTokenEndpointUris("/Account/token");

                c.AllowPasswordFlow();
                c.AllowRefreshTokenFlow();

                c.RegisterScopes(OpenIddictConstants.Scopes.Roles);

                // Don't require client_id
                c.AcceptAnonymousClients();

                if (Environment.IsDevelopment())
                {
                    // c.DisableHttpsRequirement();
                    // c.AddEphemeralSigningKey(); ??
                    c.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
                }

                c.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

                // Not used when using data protection APIs?
                c.AddEphemeralEncryptionKey();
                c.AddEphemeralSigningKey();

                c.UseDataProtection();
            })
            .AddValidation(builder =>
            {
                builder.UseLocalServer();
                builder.UseAspNetCore();

                builder.UseDataProtection();

                builder.AddEventHandler<OpenIddictValidationEvents.ProcessAuthenticationContext>(options =>
                {
                    options.AddFilter<OpenIddictValidationAspNetCoreHandlerFilters.RequireHttpRequest>();
                    options.SetOrder(OpenIddictValidationAspNetCoreHandlers.ExtractAccessTokenFromQueryString
                        .Descriptor.Order + 1_000);
                    options.UseInlineHandler(
                        context =>
                        {
                            var request = context.Transaction.GetHttpRequest();
                            if (request is null)
                            {
                                throw new InvalidOperationException("Could not get HttpRequest");
                            }

                            if (string.IsNullOrEmpty(context.Token))
                            {
                                context.Token = request.Cookies["bearer_token"];
                                context.TokenType = OpenIddictConstants.TokenTypeHints.AccessToken;
                            }

                            if (string.IsNullOrEmpty(context.Token))
                            {
                                context.Token = request.Query["access_token"];
                                context.TokenType = OpenIddictConstants.TokenTypeHints.AccessToken;
                            }

                            return default;
                        });
                });
            });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
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
                config.UserIdProvider = request => request.HttpContext.User.Identity.Name;
                config.ResultsAuthorize = request =>
                {
                    return request.HttpContext.User.IsInRole("admin");
                };
                config.ResultsListAuthorize = request =>
                {
                    return request.HttpContext.User.IsInRole("admin");
                };
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

            if (RunningUnderTest)
            {
                x.UseMemoryStorage();
            }
            else
            {
                x.UseSqlServerStorage(Configuration["DBConnection"],
                    new SqlServerStorageOptions() { });
            }

            x.UseSerializerSettings(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        });
        services.AddHangfireServer(options =>
        {
            options.Queues = new[] { JobQueues.Critical, JobQueues.Normal };
            options.WorkerCount = 3;
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
            .AddSignalR(options => { options.EnableDetailedErrors = true; })
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
            .Where(a => (a.Name ?? "").Contains("ImperaPlus", StringComparison.OrdinalIgnoreCase))
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

        // App is hosted under [dev.]imperaonline.de/api, reverse-proxied via nginx
        app.UsePathBase("/api");

        LogManager.Configuration.Variables["configDir"] = Configuration["LogDir"];

#if DEBUG
        app.UseDeveloperExceptionPage();
#endif

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });


        // Enable Cors
        app.UseCors(b => b
            .WithOrigins("http://localhost:8080", "https://dev.imperaonline.de", "https://imperaonline.de",
                "https://www.imperaonline.de", "http://127.0.0.1:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-MiniProfiler-Ids")
            .AllowCredentials());

        app.UseStaticFiles();

        // Configure swagger generation & UI
        app.UseOpenApi();
        app.UseSwaggerUi3();

        app.UseRouting();

        // Auth
        app.UseAuthentication();
        app.UseAuthorization();

        // Add profiler support
        app.UseMiniProfiler();

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

            endpoints.MapMiniProfilerIncludes(new RenderOptions
            {
                StartHidden = true,
                PopupToggleKeyboardShortcut = "Ctrl+m",
                ShowControls = true
            });


            endpoints.MapHub<MessagingHub>("/signalr/chat");
            endpoints.MapHub<GameHub>("/signalr/game");
        });

        // Initialize database
        Log.Info().Message("Initializing database...").Write();
        if (env.IsDevelopment())
        {
            if (RunningUnderTest)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            else
            {
                dbContext.Database.Migrate();
            }
        }
        else
        {
            Log.Info().Message("Starting migration...").Write();
            dbContext.Database.Migrate();
            Log.Info().Message("...done.").Write();
        }

        Log.Info().Message("...done.").Write();

        Log.Info().Message("Seeding database...").Write();
        dbSeed.Seed(dbContext).Wait();
        Log.Info().Message("...done.").Write();

        // Hangfire
        app.UseHangfireDashboard("/admin/Hangfire",
            new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });

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
        builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGameRepository))!)
            .Where(x => x.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase) && !x.IsInterface)
            .As(x => x.GetInterfaces());

        // Notification
        builder.RegisterType<GamePushNotificationService>().AsImplementedInterfaces();
        builder.RegisterType<UserPushNotificationService>().AsImplementedInterfaces();

        var jsonSettings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };
        jsonSettings.Converters.Add(new StringEnumConverter
        {
            NamingStrategy = new DefaultNamingStrategy(),
            AllowIntegerValues = false
        });

        builder.RegisterInstance(JsonSerializer.Create(jsonSettings)).As<JsonSerializer>();

        builder.RegisterModule<DependencyInjectionModule>();
        builder.RegisterModule<Domain.DependencyInjectionModule>();

        builder.RegisterType<BackgroundJobClient>().AsImplementedInterfaces();
    }
}
