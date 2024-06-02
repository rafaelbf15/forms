using MediatR;
using System;
using Forms.Core.Services;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;

namespace Forms.Business.Events
{
    public class FormsEventHandler : INotificationHandler<FormRespondidoEvent>, INotificationHandler<EsqueciSenhaEvent>
    {
        private readonly IEmailSender _emailSender;

        public FormsEventHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Handle(FormRespondidoEvent mensagem, CancellationToken cancellationToken)
        {
            
        }

        public async Task Handle(EsqueciSenhaEvent notification, CancellationToken cancellationToken)
        {
            
        }
    }
}
