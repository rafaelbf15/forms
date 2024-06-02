using Forms.Core.DomainObjects;
using System;

namespace Forms.Business.Models
{
    public class ResponsavelRecebimento : Entity
    {
        public Guid FormularioId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }


        //EF Rel
        public Formulario Formulario { get; set; }

        public ResponsavelRecebimento(Guid formularioId, Guid usuarioId, string nome, string email)
        {
            FormularioId = formularioId;
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
        }

        protected ResponsavelRecebimento() { }

        public void DefinirFormId(Guid formId)
        {
            FormularioId = formId;
        }
    }
}
