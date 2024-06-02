using Microsoft.Extensions.DependencyInjection;
using Forms.API.Extensions;
using Forms.Core.DomainObjects;
using Microsoft.AspNetCore.Http;
using Forms.API.Data;
using Forms.API.Services;
using Forms.Business.Events;
using Forms.Data;
using Forms.Core.Communication.Mediator;
using Forms.Core.Messages.CommonMessages.Notifications;
using Forms.Business.Interfaces;
using Forms.Business.Services;
using Forms.Data.Repository;
using Forms.Core.Services;
using MediatR;

namespace Forms.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //Auth
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<IUser, AspNetUser>();

            //Contexts
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<FormularioContext>();

            //Repository
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IFormularioRepository, FormularioRepository>();
          
            // Services
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IFormularioService, FormularioService>();
            services.AddScoped<IUser, AspNetUser>();

            // Notifications
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<INotificationHandler<FormRespondidoEvent>, FormsEventHandler>();
            services.AddScoped<INotificationHandler<EsqueciSenhaEvent>, FormsEventHandler>();
        }
    }
}
