using LineList.Cenovus.Com.Common;
using LineList.Cenovus.Com.Infrastructure.Context;
using LineList.Cenovus.Com.Security;
using LineList.Cenovus.Com.UI.Configuration;
using LineList.Cenovus.Com.UI.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LineList.Cenovus.Com.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<CurrentUser>();
            services.AddScoped<IAuthorizationHandler, AccessRequirementHandler>();

            // Setup the database connection
            services.AddDbContext<LineListDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppConnectionString"), options =>
                {
                    options.EnableRetryOnFailure();
                    options.CommandTimeout(480);
                });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Scoped);

            // ⛔️ Skip Azure AD if running in Development
            var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Development", StringComparison.OrdinalIgnoreCase) ?? false;
            if (!isDev)
            {
                services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

                services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Home/Unauthorized";
                });
            }

            // Set up authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Contributor", policy => policy.Requirements.Add(
                    new AccessRequirements(services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>(),
                    new RoleType[] { RoleType.Editor, RoleType.Administrator, RoleType.Contributor })));

                options.AddPolicy("Editor", policy => policy.Requirements.Add(
                    new AccessRequirements(services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>(),
                    new RoleType[] { RoleType.Editor, RoleType.Administrator })));

                options.AddPolicy("Admin", policy => policy.Requirements.Add(
                    new AccessRequirements(services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>(),
                    new RoleType[] { RoleType.Administrator })));
            });

            services.AddRazorPages().AddMicrosoftIdentityUI();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IMemoryCache, MemoryCache>();

            services.AddAutoMapper(typeof(Startup));

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = int.MaxValue;
            });

            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new NewtonsoftJsonValidationMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.ResolveDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // 🔓 Inject fake user in development
            if (env.IsDevelopment())
            {
                app.Use(async (context, next) =>
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "DEV\\testuser"),
                        new Claim(ClaimTypes.Email, "testuser@local"),
                        new Claim(ClaimTypes.Role, "APP-LineList-AllUsers-Dynamic"),
                        new Claim(ClaimTypes.Role, "LL_PRD_IODFIELD_CL"),
                        new Claim(ClaimTypes.Role, "LL_PRD_IODFIELD_FC"),
                        new Claim(ClaimTypes.Role, "LL_PRD_IODFIELD_CL_ADM"),
                        new Claim(ClaimTypes.Role, "LL_PRD_IODFIELD_FC_ADM"),
                        new Claim(ClaimTypes.Role, "LL-IOD-ADM-TQA"),
                    }, "Development");

                    context.User = new ClaimsPrincipal(identity);
                    await next.Invoke();
                });
            }

            app.UseAuthentication();
            app.UseSession();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                var sessionTracker = context.RequestServices.GetRequiredService<SessionTracker>();

                if (context.Session.GetString("SessionId") == null)
                {
                    var sessionId = Guid.NewGuid().ToString();
                    context.Session.SetString("SessionId", sessionId);
                    sessionTracker.AddSession(sessionId);
                }

                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=LineDBSearch}/{action=Index}/{id?}");
            });

        }
    }
}
