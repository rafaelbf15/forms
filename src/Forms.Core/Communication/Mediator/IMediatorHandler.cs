using Forms.Core.Messages;
using Forms.Core.Messages.CommonMessages.DomainEvents;
using System.Threading.Tasks;

namespace Forms.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;

        Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent;

    }
}
