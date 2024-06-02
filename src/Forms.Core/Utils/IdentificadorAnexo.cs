using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Core.Utils
{
    public class IdentificadorAnexo
    {
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public string Tamanho { get; set; }

        public IdentificadorAnexo(string nome, string extensao, string tamanho)
        {
            Nome = nome;
            Extensao = extensao;
            Tamanho = tamanho;
        }
    }
}
