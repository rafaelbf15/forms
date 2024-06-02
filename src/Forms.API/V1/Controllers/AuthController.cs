using System;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Forms.API.Controllers;
using Forms.API.Extensions;
using Forms.API.Models;
using Forms.API.Services;
using Forms.Business.Events;
using Forms.Core.Communication.Mediator;
using Forms.Core.DomainObjects;
using Forms.Core.Messages.CommonMessages.Notifications;
using Forms.Core.Options;
using Forms.Core.Services;
using Forms.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Forms.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : MainController
    {
        private readonly AuthenticationService _authenticationService;

        public AuthController(AuthenticationService authenticationService,
                              IUser appUser,
                              IMapper autoMapper,
                              INotificador notificador,
                              IMediatorHandler mediatorHandler,
                              IOptions<AppSettingsConfig> appSettings
            ) : base(appUser, autoMapper, notificador, mediatorHandler, appSettings)  
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new ApplicationUser
            {
                Name = usuarioRegistro.Nome,
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true,
                Cpf = usuarioRegistro.Cpf,
                CountryCode = "+55",
                Ativo = true
            };

            var result = await _authenticationService.UserManager.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            {
                await _authenticationService.UserManager.AddToRoleAsync(user, "admin");

                await _authenticationService.SignInManager.SignInAsync(user, false);
                return CustomResponse(await _authenticationService.GerarJwt(user));
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _authenticationService.SignInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha,
                false, true);

            if (result.Succeeded)
            {
                var user = await _authenticationService.UserManager.FindByEmailAsync(usuarioLogin.Email);

                if (!user.Ativo)
                {
                    AdicionarErroProcessamento("Usuário bloqueado");

                    return CustomResponse();
                }

                return CustomResponse(await _authenticationService.GerarJwt(null, usuarioLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha incorretos");
            return CustomResponse();
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var token = await _authenticationService.ObterRefreshToken(model.Refresh);

            if (token is null)
            {
                AdicionarErroProcessamento("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GerarJwt(null, token.Username));
        }


        [HttpPost("esqueci-minha-senha")]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueciSenha(EsqueciSenhaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _authenticationService.UserManager.FindByEmailAsync(viewModel.Email);

            if (user == null) return CustomResponse();

            var key = Utils.GerarCodigo(6);

            user.SetNewKeyUser(key);

            var result = await _authenticationService.UserManager.UpdateAsync(user);

            if (result.Succeeded) await PublicarEventoEsqueciSenha(Guid.Parse(user.Id), user.Email, user.Name, key);

            return CustomResponse();
        }

        private async Task PublicarEventoEsqueciSenha(Guid userId, string email, string nomeUsuario, string key)
        {
            var evento = new EsqueciSenhaEvent(userId, email, nomeUsuario, key, _appSettings.EmailFrom, _appSettings.NomeFrom);
            await _mediatorHandler.PublicarDomainEvent(evento);
        }

        [HttpPut("alterar-minha-senha/link")]
        [AllowAnonymous]
        public async Task<IActionResult> AlterarSenha(AlteraSenhaLinkViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _authenticationService.UserManager.FindByKeyAsync(viewModel.Key);

            if (user == null)
            {
                NotificarErro("Código inválido, tente novamente ou solicite um novo reset de senha!");
                return CustomResponse();
            }

            var token = await _authenticationService.UserManager.GeneratePasswordResetTokenAsync(user);

            var updatedUser = await _authenticationService.UserManager.ResetPasswordAsync(user, token, viewModel.Password);

            if (!updatedUser.Succeeded)
            {
                NotificarErro("Erro ao alterar senha, tente novamente ou contate o suporte!");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GerarJwt(user));
        }

        [HttpPut("alterar-minha-senha")]
        public async Task<IActionResult> AlterarSenha(AlteraSenhaViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                NotificarErro("Senhas não conferem!");
                return CustomResponse();
            }

            var user = await _authenticationService.UserManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                NotificarErro("Código inválido, tente novamente ou solicite um novo reset de senha!");
                return CustomResponse();
            }

            bool passwordValid = await _authenticationService.UserManager.CheckPasswordAsync(user, viewModel.ActualPassword);

            if (!passwordValid)
            {
                NotificarErro("Senha atual inválida!");
                return CustomResponse();
            }

            var token = await _authenticationService.UserManager.GeneratePasswordResetTokenAsync(user);

            var updatedUser = await _authenticationService.UserManager.ResetPasswordAsync(user, token, viewModel.Password);

            if (!updatedUser.Succeeded)
            {
                NotificarErro("Erro ao alterar senha, tente novamente ou contate o suporte!");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GerarJwt(user));
        }

        [HttpPost("valida-chave")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidarChaveAtivacaoUsuario([FromQuery] string key)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _authenticationService.UserManager.FindByKeyAsync(key);

            if (user == null)
            {
                NotificarErro("Não foi possível ativar usuário, contate o suporte!");
                return CustomResponse();
            }

            if (!user.ExpiresKey.HasValue)
            {
                NotificarErro("Convite expirado, solicite um novo!");
                return CustomResponse();
            }

            var conviteValido = user.ExpiresKey.Value.Date > DateTime.Now.Date;

            if (!conviteValido)
            {
                NotificarErro("Convite expirado, solicite um novo!");
                return CustomResponse();
            }

            return CustomResponse(conviteValido);
        }

        [HttpPost("role")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateRole([FromBody] string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                AdicionarErroProcessamento("Role inválido");
                return CustomResponse();
            }

            var result = await _authenticationService.RoleManager.CreateAsync(new IdentityRole(role));

            if (result.Succeeded)
            {
                return CustomResponse();
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }
    }
}