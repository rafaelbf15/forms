using Asp.Versioning;
using Forms.Core.DomainObjects;
using Forms.Core.Services;
using Forms.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Forms.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration, bool tests = false)
        {
            if (!tests)
            {
                var connnectionString = configuration["ConnectionStrings:DefaultConnection"];
                var serverVersion = new MySqlServerVersion(new Version(8, 2, 0));

                services.AddDbContext<FormularioContext>(options =>
                     options.UseMySql(connnectionString, serverVersion, opt => opt.EnableRetryOnFailure()));

            } else
            {
                services.AddDbContext<FormularioContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "DbTests"));
            }

            var storageConnection = configuration["AzureStorageConnection"];
            services.AddScoped<IFileClient, AzureBlobFileClient>(client =>
            {
                return new AzureBlobFileClient(storageConnection);
            });

            var sendGridSettings = configuration.GetSection("SendGridSettings");
            services.Configure<EmailAuthOptions>(sendGridSettings);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR"), new CultureInfo("pt-BR") };
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            // services.AddControllers();

            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddEndpointsApiExplorer();

            services.AddDirectoryBrowser();

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddApiExplorer(options =>
                {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                              .AllowAnyMethod()
                                                                              .AllowAnyHeader()));
            services.AddSignalR();

            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env, ConfigurationManager configuration)
        {
            configuration
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
