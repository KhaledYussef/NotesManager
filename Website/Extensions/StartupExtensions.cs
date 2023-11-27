using Core.Domains.Auth;

using Data.DbContext;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Pal.Services.FileManager;

using Services.LoggerService;

using System.Text;

namespace Website.Extensions
{
    public static class StartupExtensions
    {
        public static void AddMyDbContext(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();

        }


        //---------------------------------------------------------------------------
        public static void AddMyIdentity(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddRoles<IdentityRole>()
                  .AddEntityFrameworkStores<AppDbContext>()
                  .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Admin/Dashboard/AccessDenied");
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
            });
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = IdentityConstants.ApplicationScheme;
                option.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
            });

        }


        //---------------------------------------------------------------------------
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IFileManagerService, FileManagerService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));

        }
    }
}
