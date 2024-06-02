using Forms.Core.Helpers;
using Forms.Core.Messages.CommonMessages.Notifications;
using Forms.Core.Services;
using Forms.Business.Events;
using Forms.Business.Interfaces;
using Forms.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Business.Services
{
    public class FormularioService : BaseService, IFormularioService
    {

        private readonly IFormularioRepository _formsRepository;

        public FormularioService(IFormularioRepository formsRepository, INotificador notificador) : base(notificador)
        {
            _formsRepository = formsRepository;
        }

        public async Task<bool> AdicionarFormulario(Formulario formulario)
        {
            if (!formulario.Perguntas.IsAny())
            {
                Notificar("O formulário precisa possuir pelo menos 01 pergunta!");
                return false;
            }

            if (!formulario.ResponsaveisRecebimento.IsAny())
            {
                Notificar("O formulário precisa possuir pelo menos 01 pessoa responsável pelo recebimento!");
                return false;
            }

            _formsRepository.AdicionarFormulario(formulario);

            if (formulario.Perguntas.Any())
            {
                foreach (Pergunta pergunta in formulario.Perguntas)
                {
                    pergunta.DefinirFormId(formulario.Id);
                }
                _formsRepository.AdicionarPerguntas(formulario.Perguntas);
            }

            if (formulario.ResponsaveisRecebimento.Any())
            {
                foreach (ResponsavelRecebimento responsavel in formulario.ResponsaveisRecebimento)
                {
                    responsavel.DefinirFormId(formulario.Id);
                }
                _formsRepository.AdicionarResponsaveisRecebimentos(formulario.ResponsaveisRecebimento);
            }

            var unitOfWork = _formsRepository.UnitOfWork;

            if (unitOfWork == null) return false;

            return await unitOfWork.Commit();
        }

        public async Task<bool> AtualizarFormulario(Formulario formulario)
        {

            var responsaveisAntigos = await _formsRepository.ObterResponsaveisRecebimentosPorFormId(formulario.Id);

            _formsRepository.RemoverResponsaveisRecebimentos(responsaveisAntigos);

            _formsRepository.AtualizarFormulario(formulario);

            // _formsRepository.AtualizarResponsaveisRecebimentos(formulario.ResponsaveisRecebimento);
           
            return await _formsRepository.UnitOfWork.Commit();
        }

        public async Task<bool> RemoverFormulario(Formulario formulario)
        {
            _formsRepository.RemoverFormulario(formulario);

            return await _formsRepository.UnitOfWork.Commit();
        }


        public async Task<bool> AdicionarPerguntas(IEnumerable<Pergunta> perguntas)
        {
            _formsRepository.AdicionarPerguntas(perguntas);

            return await _formsRepository.UnitOfWork.Commit();
        }

        public async Task<bool> AlterarPergunta(Pergunta pergunta)
        {
            _formsRepository.AtualizarPergunta(pergunta);

            return await _formsRepository.UnitOfWork.Commit();
        }

        public async Task<bool> RemoverPergunta(Pergunta pergunta)
        {
            _formsRepository.RemoverPergunta(pergunta);

            return await _formsRepository.UnitOfWork.Commit();
        }

        public async Task<bool> RemoverPerguntas(IEnumerable<Pergunta> perguntas)
        {
            _formsRepository.RemoverPerguntas(perguntas);

            return await _formsRepository.UnitOfWork.Commit();
        }

        public async Task<bool> AdicionarRespostas(IEnumerable<Resposta> respostas, IEnumerable<ResponsavelRecebimento> responsaveisRecebimento, string tituloForm)
        {

            var respostaContext = respostas.FirstOrDefault();

            _formsRepository.AdicionarRespostas(respostas);

            
            respostaContext.AdicionarEvento(new FormRespondidoEvent(respostaContext.IdFormulario, 
                                                                    respostaContext.ResponsavelCadastro,
                                                                    responsaveisRecebimento.ToList(),
                                                                    tituloForm, 
                                                                    respostaContext.DataPreenchimento));

            return await _formsRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _formsRepository?.Dispose();
        }
    }
}
