using Forms.Core.Data;
using Forms.Core.DomainObjects;
using Forms.Business.DTO;
using Forms.Business.Interfaces;
using Forms.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Data.Repository
{
    public class FormularioRepository : IFormularioRepository
    {

        private readonly FormularioContext _context;
        private readonly Guid _user;

        public FormularioRepository(FormularioContext context, IUser appUser)
        {
            _context = context;
            _user = appUser.GetUserId();
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AdicionarAnexo(AnexoForms anexo)
        {
            _context.Anexos.Add(anexo);
        }

        public void AdicionarAnexos(IEnumerable<AnexoForms> anexos)
        {
            _context.Anexos.AddRange(anexos);
        }

        public void AdicionarFormulario(Formulario formulario)
        {
            formulario.DefinirResponsavelCadastro(_user);
            _context.Formularios.Add(formulario);
        }

        public void AdicionarPergunta(Pergunta pergunta)
        {
            _context.Perguntas.Add(pergunta);
        }

        public void AdicionarPerguntas(IEnumerable<Pergunta> perguntas)
        {
            _context.Perguntas.AddRange(perguntas);
        }

        public void AdicionarResponsaveisRecebimentos(IEnumerable<ResponsavelRecebimento> responsaveisRecebimentos)
        {
            _context.ResponsaveisRecebimentos.AddRange(responsaveisRecebimentos);
        }

        public void AdicionarResponsavelRecebimento(ResponsavelRecebimento responsavelRecebimento)
        {
            _context.ResponsaveisRecebimentos.Add(responsavelRecebimento);
        }

        public void AdicionarResposta(Resposta resposta)
        {
            _context.Respostas.Add(resposta);
        }

        public void AdicionarRespostas(IEnumerable<Resposta> respostas)
        {
            _context.Respostas.AddRange(respostas);
        }

        public void AtualizarAnexo(AnexoForms anexo)
        {
            _context.Anexos.Update(anexo);
        }

        public void AtualizarAnexos(IEnumerable<AnexoForms> anexos)
        {
            _context.Anexos.UpdateRange(anexos);
        }

        public void AtualizarFormulario(Formulario formulario)
        {
            formulario.DefinirResponsavelAlteracao(_user);
            _context.Formularios.Update(formulario);
        }

        public void AtualizarPergunta(Pergunta pergunta)
        {
            _context.Perguntas.Update(pergunta);
        }

        public void AtualizarPerguntas(IEnumerable<Pergunta> perguntas)
        {
            _context.Perguntas.UpdateRange(perguntas);
        }

        public void AtualizarResponsaveisRecebimentos(IEnumerable<ResponsavelRecebimento> responsaveisRecebimentos)
        {
            _context.ResponsaveisRecebimentos.UpdateRange(responsaveisRecebimentos);
        }

        public void AtualizarResponsavelRecebimento(ResponsavelRecebimento responsavelRecebimento)
        {
            _context.ResponsaveisRecebimentos.Update(responsavelRecebimento);
        }

        public void AtualizarResposta(Resposta resposta)
        {
            _context.Respostas.Update(resposta);
        }

        public void AtualizarRespostas(IEnumerable<Resposta> respostas)
        {
            _context.Respostas.UpdateRange(respostas);
        }


        public async Task<AnexoForms> ObterAnexoPorId(Guid id)
        {
            return await _context.Anexos.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AnexoForms>> ObterAnexos()
        {
            return await _context.Anexos.AsNoTracking().ToListAsync();
        }

        public async Task<Formulario> ObterFormularioPorId(Guid id)
        {
            var formulario = await _context.Formularios.AsNoTracking()
                .Include(f => f.ResponsaveisRecebimento)
                .Where(f => f.DataExclusao == DateTime.MinValue)
                .FirstOrDefaultAsync(a => a.Id == id);


            var perguntas = await _context.Perguntas.AsNoTracking()
                .Include(f => f.Respostas).ThenInclude(r => r.Anexos)
                .Where(f => f.FormularioId == id && f.DataExclusao == DateTime.MinValue).ToListAsync();

            formulario.DefinirPerguntas(perguntas);


            return formulario;
        }

        public async Task<IEnumerable<Resposta>> ObterRespostasPorFormId(Guid formId)
        {
            var formulario = await _context.Formularios.AsNoTracking()
                .Include(f => f.Perguntas).ThenInclude(f => f.Respostas).ThenInclude(r => r.Anexos)
                .Include(f => f.ResponsaveisRecebimento)
                .Where(f => f.DataExclusao == DateTime.MinValue)
                .FirstOrDefaultAsync(a => a.Id == formId);


            var perguntas = await _context.Perguntas.AsNoTracking()
                .Include(f => f.Respostas).ThenInclude(r => r.Anexos)
                .Where(f => f.FormularioId == formId && f.DataExclusao == DateTime.MinValue).ToListAsync();

            formulario.DefinirPerguntas(perguntas);


            return formulario.Perguntas.SelectMany(p => p.Respostas).ToList();
        }

        public async Task<List<Guid>> ObterResponsaveisCadastroFormulariosPorFormId(IEnumerable<Guid> ids)
        {
            var responsaveisId = await _context.Respostas.AsNoTracking().Where(r => ids.Contains(r.IdFormulario)).Select(r => r.ResponsavelCadastro).Distinct().ToListAsync();

            return responsaveisId;
        }

        public async Task<IEnumerable<ResponsaveisRespostas>> ObterResponsaveisRespostas(IEnumerable<Guid> ids)
        {

            var respostas = await _context.Respostas
                                            .AsNoTracking()
                                            .Where(r => ids.Contains(r.IdFormulario))
                                            .GroupBy(r => r.ResponsavelCadastro)
                                            .Select(r => new ResponsaveisRespostas 
                                            { ResponsavelResposta = r.Key, 
                                              FormulariosIds = r.Select(g => g.IdFormulario).Distinct().ToList()
                                             }).Distinct().ToListAsync();

            return respostas;
        }

        public async Task<Formulario> ObterFormularioRespostaPorId(Guid id)
        {
            return await _context.Formularios.AsNoTracking()
                .Include(f => f.ResponsaveisRecebimento)
                .Where(f => f.DataExclusao == DateTime.MinValue)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Formulario> ObterFormularioRespostasPorIdResponsavel(Guid formId, Guid responsavelRespostaId, string dataPreenchimento)
        {
            var formulario = await _context.Formularios.AsNoTracking()
                .Include(f => f.ResponsaveisRecebimento)
                .Where(f => f.DataExclusao == DateTime.MinValue)
                .FirstOrDefaultAsync(a => a.Id == formId);

            if (formulario == null) return formulario;

            var perguntas = await _context.Perguntas.AsNoTracking()
                .Where(f => f.FormularioId == formId && f.DataExclusao == DateTime.MinValue).ToListAsync();

            formulario.DefinirPerguntas(perguntas);


            var perguntasId = formulario.Perguntas.Select(p => p.Id).ToList();


            var respostasQuery = _context.Respostas.AsQueryable()
                .Where(r => perguntasId.Contains(r.PerguntaId));


            if (responsavelRespostaId != Guid.Empty)
            {
                respostasQuery = respostasQuery.Where(r => r.ResponsavelCadastro == responsavelRespostaId);
            }


            if (dataPreenchimento != null)
            {
                respostasQuery = respostasQuery
                    .Where(r => r.DataPreenchimento.ToString("yyyy'-'MM'-'dd'T'HH':'mm").Contains(dataPreenchimento));
            }

            var respostas = await respostasQuery.AsNoTracking().Include(r => r.Anexos).ToListAsync();

            if (respostas.Any())
            {
                foreach (var pergunta in formulario.Perguntas)
                {
                    pergunta.DefinirRespostas(respostas.Where(r => r.PerguntaId == pergunta.Id).ToList());
                }
            }
            

            return formulario;
        }

        public async Task<IEnumerable<DateTime>> ObterDatasPreenchimentoPorFormIdTexto(Guid formId, string textoPesquisa)
        {
            var formulario = await _context.Formularios.AsNoTracking()
                .Include(f => f.ResponsaveisRecebimento)
                .Where(f => f.DataExclusao == DateTime.MinValue)
                .FirstOrDefaultAsync(a => a.Id == formId);

            if (formulario == null) return Enumerable.Empty<DateTime>();

            var perguntas = await _context.Perguntas.AsNoTracking()
                .Where(f => f.FormularioId == formId && f.DataExclusao == DateTime.MinValue).ToListAsync();

            formulario.DefinirPerguntas(perguntas);

            var perguntasId = formulario.Perguntas.Select(p => p.Id).ToList();

            var dataPreenchimento = _context.Respostas.AsQueryable()
                .Where(r => perguntasId.Contains(r.PerguntaId));

            if (!string.IsNullOrEmpty(textoPesquisa))
            {
                dataPreenchimento = dataPreenchimento.Where(r => r.Texto.ToLower().Contains(textoPesquisa.ToLower()));
            }

            var datas = await dataPreenchimento.AsNoTracking().Select(r => r.DataPreenchimento).Distinct().ToListAsync();

            return datas;
        }

        public async Task<IEnumerable<DateTime>> ObterDatasPreenchimentoPorFormResponsavelIds(Guid formId, Guid responsavelRespostaId)
        {
            var formulario = await _context.Formularios.AsNoTracking()
                .Include(f => f.ResponsaveisRecebimento)
                .Where(f => f.DataExclusao == DateTime.MinValue)
                .FirstOrDefaultAsync(a => a.Id == formId);

            if (formulario == null) return Enumerable.Empty<DateTime>();

            var perguntas = await _context.Perguntas.AsNoTracking()
                .Where(f => f.FormularioId == formId && f.DataExclusao == DateTime.MinValue).ToListAsync();

            formulario.DefinirPerguntas(perguntas);


            var perguntasId = formulario.Perguntas.Select(p => p.Id).ToList();

            var dataPreenchimento = _context.Respostas.AsQueryable()
                .Where(r => perguntasId.Contains(r.PerguntaId));

            if (responsavelRespostaId != Guid.Empty)
            {
                dataPreenchimento = dataPreenchimento.Where(r => r.ResponsavelCadastro == responsavelRespostaId);
            }

            var datas = await dataPreenchimento.AsNoTracking().Select(r => r.DataPreenchimento).Distinct().ToListAsync();

            return datas;
        }



        public async Task<IEnumerable<Formulario>> ObterFormularios()
        {
            return await _context.Formularios.AsNoTracking().Where(f => f.DataExclusao == DateTime.MinValue).ToListAsync();
        }


        public async Task<IEnumerable<Formulario>> ObterFormulariosMobile()
        {
            var formularios = await _context.Formularios.AsNoTracking().Where(f => f.DataExclusao == DateTime.MinValue).ToListAsync();

            if (formularios.Any())
            {
                var perguntas = await _context.Perguntas.AsNoTracking()
                    .Where(p => formularios.Select(f => f.Id).Contains(p.FormularioId) 
                    && p.DataExclusao == DateTime.MinValue).ToListAsync();

                formularios.ForEach(f => { f.DefinirPerguntas(perguntas.Where(p => p.FormularioId == f.Id).ToList()); });
            }

            return formularios;
        }




        public async Task<Pergunta> ObterPerguntaPorId(Guid id)
        {
            return await _context.Perguntas.AsNoTracking()
                .Include(p => p.Respostas).ThenInclude(r => r.Anexos)
                .Where(p => p.DataExclusao == DateTime.MinValue)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Pergunta>> ObterPerguntas()
        {
            return await _context.Perguntas.AsNoTracking()
                .Where(p => p.DataExclusao == DateTime.MinValue)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResponsavelRecebimento>> ObterResponsaveisRecebimentos()
        {
            return await _context.ResponsaveisRecebimentos.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ResponsavelRecebimento>> ObterResponsaveisRecebimentosPorFormId(Guid formId)
        {
            return await _context.ResponsaveisRecebimentos.AsNoTracking()
                .Where(r => r.FormularioId == formId)
                .ToListAsync();
        }

        public async Task<ResponsavelRecebimento> ObterResponsavelRecebimentoPorId(Guid id)
        {
            return await _context.ResponsaveisRecebimentos.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Resposta> ObterRespostaPorId(Guid id)
        {
            return await _context.Respostas
                .Include(r => r.Anexos)
                .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Resposta>> ObterRespostas()
        {
            return await _context.Respostas.AsNoTracking().ToListAsync();
        }

        public void RemoverAnexo(AnexoForms anexo)
        {
            _context.Anexos.Remove(anexo);
        }

        public void RemoverAnexos(IEnumerable<AnexoForms> anexos)
        {
            _context.Anexos.RemoveRange(anexos);
        }

        public void RemoverFormulario(Formulario formulario)
        {
            formulario.DefinirResponsavelExclusao(_user);

            _context.Formularios.Update(formulario);
            // _context.Formularios.Remove(formulario);
        }

        public void RemoverPergunta(Pergunta pergunta)
        {
            pergunta.DefinirResponsavelExclusao(_user);

            _context.Perguntas.Update(pergunta);
        }

        public void RemoverPerguntas(IEnumerable<Pergunta> perguntas)
        {
            foreach (var pergunta in perguntas)
            {
                pergunta.DefinirResponsavelExclusao(_user);
            }
            _context.Perguntas.UpdateRange(perguntas);
        }

        public void RemoverResponsaveisRecebimentos(IEnumerable<ResponsavelRecebimento> respoResponsaveisRecebimentosstas)
        {
            _context.ResponsaveisRecebimentos.RemoveRange(respoResponsaveisRecebimentosstas);
        }

        public void RemoverResponsavelRecebimento(ResponsavelRecebimento responsavelRecebimento)
        {
            _context.ResponsaveisRecebimentos.Remove(responsavelRecebimento);
        }

        public void RemoverResposta(Resposta resposta)
        {
            _context.Respostas.Remove(resposta);
        }

        public void RemoverRespostas(IEnumerable<Resposta> respostas)
        {
            _context.Respostas.RemoveRange(respostas);
        }


        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
