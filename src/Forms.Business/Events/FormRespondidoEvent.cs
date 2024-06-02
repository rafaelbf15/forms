using Forms.Core.Messages.CommonMessages.DomainEvents;
using Forms.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Business.Events
{
    public class FormRespondidoEvent : DomainEvent
    {

        public List<ResponsavelRecebimento> ResponsaveisRecebimentos { get; private set; }
        public Guid ResponsavelCadastro { get; private set; }
        public string Titulo { get; private set; }
        public DateTime DataPreenchimento { get; private set; }
        public FormRespondidoEvent(Guid aggregateId,
                                   Guid responsavelCadastro,
                                   List<ResponsavelRecebimento> responsaveisRecebimentos, 
                                   string titulo, 
                                   DateTime dataPreenchimento) : base(aggregateId)
        {
            AggregateId = aggregateId;
            ResponsavelCadastro = responsavelCadastro;
            ResponsaveisRecebimentos = responsaveisRecebimentos;
            Titulo = titulo;
            DataPreenchimento = dataPreenchimento;
        }
    }
}
