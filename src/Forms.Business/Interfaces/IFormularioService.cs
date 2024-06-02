using Forms.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Business.Interfaces
{
    public interface IFormularioService : IDisposable
    {
        Task<bool> AdicionarFormulario(Formulario formulario);
        Task<bool> AtualizarFormulario(Formulario formulario);
        Task<bool> RemoverFormulario(Formulario formulario);


        Task<bool> AdicionarPerguntas(IEnumerable<Pergunta> perguntas);
        Task<bool> AlterarPergunta(Pergunta pergunta);
        Task<bool> RemoverPergunta(Pergunta pergunta);
        Task<bool> RemoverPerguntas(IEnumerable<Pergunta> perguntas);

        Task<bool> AdicionarRespostas(IEnumerable<Resposta> respostas, IEnumerable<ResponsavelRecebimento> responsaveisRecebimento, string tituloForm);

    }
}
