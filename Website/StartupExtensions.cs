using Core.Domains.Auth;
using Microsoft.Extensions.DependencyInjection;
using Data.DbContext;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Pal.Services.Email;
using Pal.Services.FileManager;
using Pal.Web.Extensions;

using Service;

using Services.LoggerService;

using System.Text;
using Microsoft.OpenApi.Models;

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
                    ValidAudience = Configuration["JWT:Issuer"],
                    ValidIssuer = Configuration["JWT:Issuer"],
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

        public static void AddMySwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "WebAPI.xml");
                var filePath2 = Path.Combine(AppContext.BaseDirectory, "Data.xml");
                c.IncludeXmlComments(filePath);
                c.IncludeXmlComments(filePath2);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });
        }

    }
}
