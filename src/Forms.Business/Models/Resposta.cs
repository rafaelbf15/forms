using Forms.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forms.Business.Models
{
    public class Resposta: Entity
    {
        public Guid PerguntaId { get; private set; }
        public string Texto { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataPreenchimento { get; private set; }
        public Guid ResponsavelCadastro { get; private set; }
        public IEnumerable<AnexoForms> Anexos { get; private set; }
        public Guid IdFormulario { get; set; }

        //EF Rel
        public Pergunta Pergunta { get; set; }

        public Resposta(string texto, Guid responsavelCadastro, IEnumerable<AnexoForms> anexos, DateTime dataPreenchimento)
        {
            Texto = texto;
            DataPreenchimento = dataPreenchimento;
            ResponsavelCadastro = responsavelCadastro;
            Anexos = anexos;
        }

        protected Resposta() { }
    }
}
