using Forms.API.Data;
using Forms.API.Extensions;
using Forms.Core.Options;
using Forms.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;
using Forms.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Forms.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connnectionString = configuration["ConnectionStrings:DefaultConnection"];
            var serverVersion = new MySqlServerVersion(new Version(8, 2, 0));

            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseMySql(connnectionString, serverVersion, opt => opt.EnableRetryOnFailure()));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddErrorDescriber<IdentityMensagensPortugues>()
                 .AddDefaultTokenProviders();

            var appSettingsConfigSection = configuration.GetSection("AppSettingConfig");
            services.Configure<AppSettingsConfig>(appSettingsConfigSection);

            var appSettingsConfig = appSettingsConfigSection.Get<AppSettingsConfig>();
            var key = Encoding.ASCII.GetBytes(appSettingsConfig.Secret);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettingsConfig.ValidoEm,
                    ValidIssuer = appSettingsConfig.Emissor
                };
            });

            return services;
        }

    }
}
