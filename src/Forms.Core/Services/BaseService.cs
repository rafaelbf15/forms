using Forms.Core.DomainObjects;
using Forms.Core.Messages.CommonMessages.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace Forms.Core.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            try
            {
                var validator = validacao.Validate(entidade);

                if (validator.IsValid) return true;

                Notificar(validator);
            }
            catch (System.Exception)
            {

                Notificar("Objeto nulo! Não foi possível realizar a operação de validação!");
                return false;
            }

            return false;
        }
    }
}