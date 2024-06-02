using Forms.Core.Messages;
using Forms.Core.Messages.CommonMessages.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectLearn.Domain.Events
{
    public class NovoUsuarioAtivacaoEvent : DomainEvent
    {
        public string Url { get; set; }
        public string Email { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailFrom { get; set; }
        public string NomeFrom { get; set; }

        public NovoUsuarioAtivacaoEvent(Guid aggregateId, string url, string email, string nomeUsuario, string nomeFrom, string emailFrom) : base(aggregateId)
        {
            AggregateId = aggregateId;
            Email = email;
            Url = url;
            NomeUsuario = nomeUsuario;
            NomeFrom = nomeFrom;
            EmailFrom = emailFrom;
        }
    }
}
