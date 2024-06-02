using Forms.Core.Messages;
using Forms.Core.Messages.CommonMessages.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Business.Events
{
    public class EsqueciSenhaEvent : DomainEvent
    {
        public string Email { get; set; }
        public string NomeUsuario { get; set; }
        public string Key { get; set; }
        public string EmailFrom { get; set; }
        public string NomeFrom { get; set; }

        public EsqueciSenhaEvent(Guid aggregateId, string email, string nomeUsuario, string key, string emailFrom, string nomeFrom) : base(aggregateId)
        {
            Email = email;
            NomeUsuario = nomeUsuario;
            Key = key;
            EmailFrom = emailFrom;
            NomeFrom = nomeFrom;
        }
    }
}
