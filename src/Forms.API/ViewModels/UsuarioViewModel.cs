using System;
using System.Collections.Generic;

namespace Forms.API.Models
{
    public class UsuarioViewModel
    {
        public Guid? Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string IdentificadorArquivo { get; set; }
        public bool Ativo { get; set; }
        public Guid? IdentityUserId { get; set; }
    }
}
