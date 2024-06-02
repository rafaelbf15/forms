using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Forms.Core.Communication;
using Forms.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forms.API.Models;
using Microsoft.Extensions.Options;
using Forms.Core.DomainObjects;
using Forms.Core.Services;
using Forms.Core.Options;
using Forms.Core.Messages.CommonMessages.Notifications;
using Forms.Core.Communication.Mediator;

namespace Forms.API.Controllers
{

    [ApiController]
    public class MainController : ControllerBase
    {
        public readonly IUser AppUser;
        protected readonly IMapper _autoMapper;
        protected readonly INotificador _notificador;
        protected readonly Guid userId;
        protected readonly string role;
        protected readonly IMediatorHandler _mediatorHandler;
        protected readonly AppSettingsConfig _appSettings;

        public MainController(IUser appUser, 
                              IMapper autoMapper, 
                              INotificador notificador,
                              IMediatorHandler mediatorHandler,
                              IOptions<AppSettingsConfig> appSettings
            )
        {
            AppUser = appUser;
            _autoMapper = autoMapper;
            _notificador = notificador;
            _mediatorHandler = mediatorHandler;
            _appSettings = appSettings.Value;
            userId = appUser.GetUserId();
            role = appUser.GetUserRole();
        }

        protected List<string> Erros = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            AtribuirNotificacoes();

            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ResponseResult resposta)
        {
            ResponsePossuiErros(resposta);

            return CustomResponse();
        }

        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta == null || !resposta.Errors.Mensagens.Any()) return false;

            foreach (var mensagem in resposta.Errors.Mensagens)
            {
                AdicionarErroProcessamento(mensagem);
            }

            return true;
        }

        protected bool OperacaoValida()
        {
            return !Erros.IsAny();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            Erros.Add(erro);
        }

        protected void LimparErrosProcessamento()
        {
            Erros.Clear();
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        private void AtribuirNotificacoes()
        {
            if (_notificador.TemNotificacao())
            {
                Erros.AddRange(_notificador.ObterNotificacoes().Select(n => n.Mensagem));
            }  
        }
    }
}
