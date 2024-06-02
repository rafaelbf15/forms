using Forms.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace Forms.Business.Models
{
    public class Pergunta: Entity
    {
        public Guid FormularioId { get; private set; }
        public string Titulo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public DateTime DataExclusao { get; private set; }
        public Guid ResponsavelCadastro { get; private set; }
        public Guid ResponsavelAlteracao { get; private set; }
        public Guid ResponsavelExclusao { get; private set; }
        public bool AnexoObrigatorio { get; private set; }
        public bool TextoObrigatorio { get; private set; }
        public IEnumerable<Resposta> Respostas { get; private set; }

        //EF Rel
        public Formulario Formulario { get; set; }

        public Pergunta(string titulo, bool anexoObrigatorio, bool textoObrigatorio)
        {
            Titulo = titulo;
            AnexoObrigatorio = anexoObrigatorio;
            TextoObrigatorio = textoObrigatorio;
        }

        protected Pergunta() { }

        public void DefinirResponsavelExclusao(Guid usuarioId)
        {
            ResponsavelExclusao = usuarioId;
            DataExclusao = DateTime.UtcNow;
        }

        public void DefinirResponsavelAlteracao(Guid usuarioId)
        {
            ResponsavelAlteracao = usuarioId;
            DataAlteracao = DateTime.UtcNow;
        }

        public void DefinirFormId(Guid formId)
        {
            FormularioId = formId;
        }

        public void DefinirRespostas(List<Resposta> respostas)
        {
            Respostas = respostas;
        }
    }
}
