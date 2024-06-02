using Microsoft.AspNetCore.Identity;
using System;

namespace Forms.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string CountryCode { get; internal set; }
        public string Key { get; set; }
        public string Cpf { get; set; }
        public bool Ativo { get; set; }
        public DateTime? ExpiresKey { get; set; }

        public ApplicationUser()
        {
        }

        public void SetNewKeyUser(string key)
        {
            Key = key;
            ExpiresKey = DateTime.UtcNow.AddDays(1);
        }

        public void SetAtivo(bool ativo)
        {
            Ativo = ativo;
        }
    }
}
