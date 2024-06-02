using Forms.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Business.Models
{
    public class Formulario : Entity, IAggregateRoot
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public DateTime DataExclusao { get; private set; }
        public Guid ResponsavelCadastro { get; private set; }
        public Guid ResponsavelAlteracao { get; private set; }
        public Guid ResponsavelExclusao { get; private set; }
        public IEnumerable<Pergunta> Perguntas { get; private set; }
        public IEnumerable<ResponsavelRecebimento> ResponsaveisRecebimento { get; private set; }
        public bool VisualizacaoTodos { get; private set; }

        public Formulario(string titulo, string descricao, IEnumerable<Pergunta> perguntas, IEnumerable<ResponsavelRecebimento> responsaveisRecebimentos)
        {
            Titulo = titulo;
            Descricao = descricao;
            Perguntas = perguntas;
            ResponsaveisRecebimento = responsaveisRecebimentos;
            VisualizacaoTodos = false;
        }

        protected Formulario() { }

        public void DefinirResponsavelCadastro(Guid usuarioId)
        {
            ResponsavelCadastro = usuarioId;
        }

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

        public void DefinirPerguntas(IEnumerable<Pergunta> perguntas)
        {
            Perguntas = perguntas;
        }
    }
}
