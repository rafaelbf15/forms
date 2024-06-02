using System;

namespace Forms.API.ViewModels.Forms
{
    public class ResponsavelRecebimentoFormsViewModel
    {
        public Guid? Id { get; set; }
        public Guid? FormularioId { get;  set; }
        public Guid? UsuarioId { get;  set; }
        public string Nome { get;  set; }
        public string Email { get;  set; }

        public ResponsavelRecebimentoFormsViewModel(Guid usuarioId, string nome, string email)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
        }
    }
}
