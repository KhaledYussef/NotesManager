using Core.Domains.Auth;

using Data.DbContext;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Pal.Services.Email;
using Pal.Services.FileManager;
using Pal.Web.Extensions;

using Service;

using Services.LoggerService;

using System.Text;

namespace Website
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
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.SignIn.RequireConfirmedAccount = false;
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.User.RequireUniqueEmail = true;
            })
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
            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<IEmailService, EmailService>();

        }


        //---------------------------------------------------------------------------
        public static void AddMyActionFilters(this IServiceCollection services)
        {
            services.AddScoped<CheckUserAttribute>();

        }

    }
}
