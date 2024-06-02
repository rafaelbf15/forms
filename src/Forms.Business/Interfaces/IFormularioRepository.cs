using Forms.Core.Data;
using Forms.Business.DTO;
using Forms.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Business.Interfaces
{
    public interface IFormularioRepository : IRepository<Formulario>
    {

        Task<IEnumerable<Formulario>> ObterFormularios();
        Task<IEnumerable<Formulario>> ObterFormulariosMobile();
        Task<Formulario> ObterFormularioPorId(Guid id);
        Task<Formulario> ObterFormularioRespostaPorId(Guid id);
        Task<Formulario> ObterFormularioRespostasPorIdResponsavel(Guid formId, Guid responsavelRespostaId, string dataPreenchimento);
        Task<IEnumerable<DateTime>> ObterDatasPreenchimentoPorFormResponsavelIds(Guid formId, Guid responsavelRespostaId);
        Task<IEnumerable<DateTime>> ObterDatasPreenchimentoPorFormIdTexto(Guid formId, string textoPesquisa);
        void AdicionarFormulario(Formulario formulario);
        void AtualizarFormulario(Formulario formulario);
        void RemoverFormulario(Formulario formulario);


        Task<IEnumerable<Pergunta>> ObterPerguntas();
        Task<Pergunta> ObterPerguntaPorId(Guid id);
        void AdicionarPergunta(Pergunta pergunta);
        void AdicionarPerguntas(IEnumerable<Pergunta> perguntas);
        void AtualizarPergunta(Pergunta pergunta);
        void AtualizarPerguntas(IEnumerable<Pergunta> perguntas);
        void RemoverPergunta(Pergunta pergunta);
        void RemoverPerguntas(IEnumerable<Pergunta> perguntas);

        Task<List<Guid>> ObterResponsaveisCadastroFormulariosPorFormId(IEnumerable<Guid> ids);
        Task<IEnumerable<ResponsaveisRespostas>> ObterResponsaveisRespostas(IEnumerable<Guid> ids);
        Task<IEnumerable<Resposta>> ObterRespostas();
        Task<IEnumerable<Resposta>> ObterRespostasPorFormId(Guid formId);
        Task<Resposta> ObterRespostaPorId(Guid id);
        void AdicionarResposta(Resposta resposta);
        void AdicionarRespostas(IEnumerable<Resposta> respostas);
        void AtualizarResposta(Resposta resposta);
        void AtualizarRespostas(IEnumerable<Resposta> respostas);
        void RemoverResposta(Resposta resposta);
        void RemoverRespostas(IEnumerable<Resposta> respostas);



        Task<IEnumerable<ResponsavelRecebimento>> ObterResponsaveisRecebimentos();
        Task<IEnumerable<ResponsavelRecebimento>> ObterResponsaveisRecebimentosPorFormId(Guid formId);
        Task<ResponsavelRecebimento> ObterResponsavelRecebimentoPorId(Guid id);
        void AdicionarResponsavelRecebimento(ResponsavelRecebimento responsavelRecebimento);
        void AdicionarResponsaveisRecebimentos(IEnumerable<ResponsavelRecebimento> responsaveisRecebimentos);
        void AtualizarResponsavelRecebimento(ResponsavelRecebimento responsavelRecebimento);
        void AtualizarResponsaveisRecebimentos(IEnumerable<ResponsavelRecebimento> responsaveisRecebimentos);
        void RemoverResponsavelRecebimento(ResponsavelRecebimento responsavelRecebimento);
        void RemoverResponsaveisRecebimentos(IEnumerable<ResponsavelRecebimento> respoResponsaveisRecebimentosstas);


        Task<IEnumerable<AnexoForms>> ObterAnexos();
        Task<AnexoForms> ObterAnexoPorId(Guid id);
        void AdicionarAnexo(AnexoForms anexo);
        void AdicionarAnexos(IEnumerable<AnexoForms> anexos);
        void AtualizarAnexo(AnexoForms anexo);
        void AtualizarAnexos(IEnumerable<AnexoForms> anexos);
        void RemoverAnexo(AnexoForms anexo);
        void RemoverAnexos(IEnumerable<AnexoForms> anexos);






    }
}
