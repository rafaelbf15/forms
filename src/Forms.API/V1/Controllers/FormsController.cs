using AutoMapper;
using Forms.API.Controllers;
using Forms.API.Services;
using Forms.API.ViewModels;
using Forms.API.ViewModels.Forms;
using Forms.Core.DomainObjects;
using Forms.Core.Helpers;
using Forms.Core.Messages.CommonMessages.Notifications;
using Forms.Core.Services;
using Forms.Business.Interfaces;
using Forms.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Forms.Core.Communication.Mediator;
using Forms.Core.Options;
using Microsoft.Extensions.Options;
using Forms.Core.Data.Queries;
using Asp.Versioning;

namespace Forms.API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/forms")]
    public class FormsController : MainController
    {

        private readonly IFormularioService _formularioService;
        private readonly IFormularioRepository _formularioRepository;
        private readonly IMapper _mapper;
        private readonly IFileClient _fileClient;
        private readonly AuthenticationService _authenticationService;


        public FormsController(IFormularioService formularioService,
                             IFormularioRepository formularioRepository,
                             IMapper mapper,
                             IFileClient fileClient,
                             INotificador notificador,
                             IUser appUser,
                             IMediatorHandler mediatorHandler,
                             IOptions<AppSettingsConfig> appSettings,
                             AuthenticationService authenticationService) 
            : base(appUser, mapper, notificador, mediatorHandler, appSettings)
        {
            _formularioService = formularioService;
            _formularioRepository = formularioRepository;
            _mapper = mapper;
            _fileClient = fileClient;
        }


        [HttpPost]
        [Route("formularios")]
        [Authorize(Roles = "ADMIN-FORMS, FORMS-ED")]
        public async Task<IActionResult> AdicionarFormulario(FormularioFormsViewModel formularioViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (formularioViewModel == null)
            {
                NotificarErro("Erro ao adicionar formulário, tente novamente!");
                return CustomResponse(formularioViewModel);
            }

            var formulario = _mapper.Map<Formulario>(formularioViewModel);

            if (!await _formularioService.AdicionarFormulario(formulario))
            {
                NotificarErro("Erro ao adicionar formulário, tente novamente!");
                return CustomResponse(formularioViewModel);
            }

            return CustomResponse(formularioViewModel);
        }

        [HttpGet]
        [Route("formularios")]
        public async Task<IActionResult> ObterFormulario()
        {

            var formularios = await _formularioRepository.ObterFormularios();

            if (!formularios.Any()) return CustomResponse();

            var formulariosViewModel = _mapper.Map<IEnumerable<FormularioFormsViewModel>>(formularios);

            return CustomResponse(formulariosViewModel);
        }

        [HttpGet]
        [Route("formularios/{formId:guid}")]
        public async Task<IActionResult> ObterFormularioPorId(Guid formId)
        {

            var formulario = await _formularioRepository.ObterFormularioPorId(formId);

            if (formulario == null)
            {
                NotificarErro("Não foi possível obter o formulário, tente novamente!");
                return CustomResponse();
            }

            var formularioViewModel = _mapper.Map<FormularioFormsViewModel>(formulario);

            return CustomResponse(formularioViewModel);
        }

        [HttpGet]
        [Route("formularios/{formId:guid}/respostas")]
        public async Task<IActionResult> ObterRespostasPorFormId(Guid formId)
        {

            var respostas = await _formularioRepository.ObterRespostasPorFormId(formId);

            if (!respostas.Any())
            {
                return CustomResponse();
            }

            var respostasViewModel = _mapper.Map<IEnumerable<RespostaFormsViewModel>>(respostas);

            var responsaveisCadastro = respostasViewModel.Select(r => r.ResponsavelCadastro).ToList();

            return CustomResponse(new { ResponsaveisCadastro = responsaveisCadastro, Respostas = respostasViewModel });
        }

        [HttpGet]
        [Route("formularios/respostas/responsaveisCadastro")]
        public async Task<IActionResult> ObterFormulariosDropdown()
        {

            var formularios = await _formularioRepository.ObterFormularios();

            if (!formularios.Any())
            {
                return CustomResponse();
            }

            var responsaveisRespostas = await _formularioRepository.ObterResponsaveisRespostas(formularios.Select(f => f.Id));

            if (!responsaveisRespostas.Any())
            {
                return CustomResponse();
            }

            var responsaveisRespostasIds = responsaveisRespostas.Select(r => r.ResponsavelResposta.ToString()).ToList();

            var usuariosFilter = await _authenticationService.GetUsersByIds(responsaveisRespostasIds);

            if (usuariosFilter.IsAny()) return CustomResponse();

            var usuariosViewModel = usuariosFilter.Select(u => 
            new { 
                Label = u.UserName, 
                Value = u.Id, 
                FormulariosIds = responsaveisRespostas.Where(r => r.ResponsavelResposta == Guid.Parse(u.Id)).SelectMany(r => r.FormulariosIds).ToList() 
            }).ToList();

            var formulariosViewModel = formularios.Select(f => new { Label = f.Titulo, Value = f.Id }).ToList();

            return CustomResponse(new { ResponsaveisRespostas = usuariosViewModel, Formularios = formulariosViewModel });
        }

        [HttpGet]
        [Route("formularios/{formId:guid}/datasPreenhcimento")]
        public async Task<IActionResult> ObterDatasPreenchimento(Guid formId, [FromQuery] Guid responsavelId)
        {

            var datasPreenhcimento = await _formularioRepository.ObterDatasPreenchimentoPorFormResponsavelIds(formId, responsavelId);

            if (!datasPreenhcimento.IsAny())
            {
                return CustomResponse();
            }

            return CustomResponse(datasPreenhcimento.Select(d => new { Label = d.AddHours(-3).ToString("dd/MM/yyyy HH:mm"), Value = d.ToString("yyyy-MM-ddTHH:mm") }).ToList());
        }


        [HttpGet]
        [Route("formularios/{formId:guid}/datasPreenhcimento/filter")]
        public async Task<IActionResult> FiltrarDatasPreenhcimentoPorTexto(Guid formId, [FromQuery] string textoPesquisa)
        {

            var datasPreenhcimento = await _formularioRepository.ObterDatasPreenchimentoPorFormIdTexto(formId, textoPesquisa);

            if (!datasPreenhcimento.IsAny())
            {
                return CustomResponse();
            }

            return CustomResponse(datasPreenhcimento.Select(d => new { Label = d.AddHours(-3).ToString("dd/MM/yyyy HH:mm"), Value = d.ToString("yyyy-MM-ddTHH:mm") }).ToList());
        }


        [HttpGet]
        [Route("formularios/{formId:guid}/respostasquery")]
        public async Task<IActionResult> ObterFormularioRespostasPorIdResponsavel(Guid formId, [FromQuery] Guid responsavelRespostaId, [FromQuery] string dataPreenchimento)
        {
           
            var formulario = await _formularioRepository.ObterFormularioRespostasPorIdResponsavel(formId, responsavelRespostaId, dataPreenchimento);

            if (formulario == null)
            {
                NotificarErro("Não foi possível obter o formulário, tente novamente!");
                return CustomResponse();
            }

            if (!formulario.VisualizacaoTodos)
            {
                if (AppUser.IsInRole("ADMIN-FORMS"))
                {
                    if (userId != formulario.ResponsavelCadastro && !(formulario.ResponsaveisRecebimento.Select(r => r.UsuarioId).Contains(userId)))
                    {
                        return Forbid();
                    }
                }

            }  

            var formularioViewModel = _mapper.Map<FormularioFormsViewModel>(formulario);

            return CustomResponse(formularioViewModel);
        }


        [HttpPut]
        [Route("formularios/{formId:guid}")]
        [Authorize(Roles = "ADMIN-FORMS, FORMS-ED")]
        public async Task<IActionResult> AtualizarFormulario(Guid formId, FormularioFormsViewModel formularioViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (formId != formularioViewModel.Id)
            {
                NotificarErro("Erro ao atualizar formulário, tente novamente!");
                return CustomResponse(formularioViewModel);
            }

            if (formularioViewModel == null)
            {
                NotificarErro("Erro ao atualizar formulário, tente novamente!");
                return CustomResponse(formularioViewModel);
            }

            var formulario = _mapper.Map<Formulario>(formularioViewModel);

            if (!await _formularioService.AtualizarFormulario(formulario))
            {
                NotificarErro("Erro ao atualizar formulário, tente novamente!");
                return CustomResponse(formularioViewModel);
            }

            return CustomResponse(formularioViewModel);
        }


        [HttpDelete]
        [Route("formularios/{formId:guid}")]
        [Authorize(Roles = "ADMIN-FORMS, FORMS-ED")]
        public async Task<IActionResult> RemoverFormulario(Guid formId)
        {
            var formulario = await _formularioRepository.ObterFormularioPorId(formId);

            if (formulario == null)
            {
                NotificarErro("Não foi possível remover o formulário, tente novamente!");
                return CustomResponse();
            }

            if (!await _formularioService.RemoverFormulario(formulario))
            {
                NotificarErro("Não foi possível remover o formulário, tente novamente!");
                return CustomResponse();
            }

            return CustomResponse();
        }


        [HttpPost]
        [Route("formularios/{formId:guid}/perguntas")]
        [Authorize(Roles = "ADMIN-FORMS, FORMS-ED")]
        public async Task<IActionResult> AdicionarPerguntas(Guid formId, List<PerguntaFormsViewModel> perguntasFormsViewModels)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (perguntasFormsViewModels == null)
            {
                NotificarErro("Erro ao adicionar perguntas, tente novamente!");
                return CustomResponse(perguntasFormsViewModels);
            }

            if (formId != perguntasFormsViewModels.FirstOrDefault().FormularioId)
            {
                NotificarErro("Erro ao adicionar perguntas, tente novamente!");
                return CustomResponse(perguntasFormsViewModels);
            }

            var perguntas = _mapper.Map<IEnumerable<Pergunta>>(perguntasFormsViewModels);

            if (!await _formularioService.AdicionarPerguntas(perguntas))
            {
                NotificarErro("Erro ao adicionar formulário, tente novamente!");
                return CustomResponse(perguntasFormsViewModels);
            }

            return CustomResponse(perguntasFormsViewModels);
        }

        [HttpPut]
        [Route("formularios/{formId:guid}/perguntas/{perguntaId:guid}")]
        [Authorize(Roles = "ADMIN-FORMS, FORMS-ED")]
        public async Task<IActionResult> AtualizarPergunta(Guid formId,  Guid perguntaId, PerguntaFormsViewModel perguntaFormsViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (perguntaFormsViewModel == null)
            {
                NotificarErro("Erro ao atualizar pergunta, tente novamente!");
                return CustomResponse(perguntaFormsViewModel);
            }

            if (perguntaId != perguntaFormsViewModel.Id)
            {
                NotificarErro("Erro ao atualizar pergunta, tente novamente!");
                return CustomResponse(perguntaFormsViewModel);
            }

            if (formId != perguntaFormsViewModel.FormularioId)
            {
                NotificarErro("Erro ao atualizar pergunta, tente novamente!");
                return CustomResponse(perguntaFormsViewModel);
            }

            var pergunta = _mapper.Map<Pergunta>(perguntaFormsViewModel);

            if (!await _formularioService.AlterarPergunta(pergunta))
            {
                NotificarErro("Erro ao atualizar pergunta, tente novamente!");
                return CustomResponse(perguntaFormsViewModel);
            }

            return CustomResponse(perguntaFormsViewModel);
        }

        [HttpDelete]
        [Route("formularios/{formId:guid}/perguntas/{perguntaId:guid}")]
        [Authorize(Roles = "ADMIN-FORMS, FORMS-ED")]
        public async Task<IActionResult> RemoverPergunta(Guid perguntaId)
        {
            var pergunta = await _formularioRepository.ObterPerguntaPorId(perguntaId);

            if (pergunta == null)
            {
                NotificarErro("Não foi possível remover a pergunta, tente novamente!");
                return CustomResponse();
            }

            if (!await _formularioService.RemoverPergunta(pergunta))
            {
                NotificarErro("Não foi possível remover a pergunta, tente novamente!");
                return CustomResponse();
            }

            return CustomResponse();
        }

        
        [HttpPost]
        [Route("formularios/{formId:guid}/respostas")]
        public async Task<IActionResult> AdicionarRespostas(Guid formId, List<RespostaFormsViewModel> respostaFormsViewModels)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var formulario = await _formularioRepository.ObterFormularioPorId(formId);

            if (formulario == null)
            {
                NotificarErro("Erro ao adicionar respostas, tente novamente!");
                return CustomResponse(respostaFormsViewModels);
            }

            if (formulario.Perguntas.FirstOrDefault().Id != respostaFormsViewModels.FirstOrDefault().PerguntaId)
            {
                NotificarErro("Erro ao adicionar respostas, tente novamente!");
                return CustomResponse(respostaFormsViewModels);
            }

            var files = new List<FileViewModel>();

            foreach (var respostaViewModel in respostaFormsViewModels)
            {
               
                if (respostaViewModel.Anexos.Any())
                {
                    
                    foreach (var item in respostaViewModel.Anexos)
                    {
                        if (item.File.Contains("image"))
                        {
                            //Normal image size
                            var path = Guid.NewGuid() + ".jpg";
                            item.Arquivo = path;
                            var fileViewModel = new FileViewModel
                            {
                                Path = path,
                                Base64 = item.File
                            };
                            files.Add(fileViewModel);

                            //ThumbnailMed image size
                            var pathThumbMedio = "thumbnailMedio_" + path;
                            item.ThumbnailMedio = pathThumbMedio;
                            var fileThumbMedioViewModel = new FileViewModel
                            {
                                Path = pathThumbMedio,
                                Base64 = item.File
                            };
                            files.Add(fileThumbMedioViewModel);

                            //Thumbnail image size
                            var pathThumb = "thumbnail_" + path;
                            item.Thumbnail = pathThumb;
                            var fileThumbViewModel = new FileViewModel
                            {
                                Path = pathThumb,
                                Base64 = item.File
                            };
                            files.Add(fileThumbViewModel);
                        }
                        else
                        {
                            if (!item.File.Contains("application/pdf"))
                            {
                                NotificarErro("O Anexo deve ser somente imagem ou pdf.");
                                return CustomResponse();
                            }
                            //Normal image size
                            var path = Guid.NewGuid() + ".pdf";
                            item.Arquivo = path;
                            var fileViewModel = new FileViewModel
                            {
                                Path = path,
                                Base64 = item.File
                            };
                            files.Add(fileViewModel);
                        }
                    }
                }

                if (files.Any())
                {
                    if (!await SalvarAnexos(files))
                    {
                        NotificarErro("Não foi possível salvar os anexos das respostas, tente novamente!");
                        return CustomResponse(respostaFormsViewModels);
                    }
                }
            }

            var respostas = _mapper.Map<IEnumerable<Resposta>>(respostaFormsViewModels);

            if (!await _formularioService.AdicionarRespostas(respostas, formulario.ResponsaveisRecebimento, formulario.Titulo))
            {
                    foreach (var item in files)
                    {
                        if (!string.IsNullOrEmpty(item.Path)) await RemoverAnexo(item.Path);
                    }
                
                NotificarErro("Erro ao adicionar respostas, tente novamente!");
                return CustomResponse(respostaFormsViewModels);
            }

            return CustomResponse(respostaFormsViewModels);
        }

        private async Task<bool> SalvarAnexos(List<FileViewModel> files)
        {

            foreach (var file in files)
            {
                var base64Data = _fileClient.IsImage(file.Path) ? Regex.Match(file.Base64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value : Regex.Match(file.Base64, @"data:application/pdf(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var binData = Convert.FromBase64String(base64Data);
                var stream = new MemoryStream(binData);

                if (!await _fileClient.SaveFile("formularios", file.Path, stream))
                {
                    NotificarErro("Falha ao salvar o anexo.");
                    return false;
                }
            }

            return true;
        }

        private async Task<ActionResult<bool>> RemoverAnexo(string file)
        {
            if (file == null)
            {
                NotificarErro("Arquivo não encontrado");
                return CustomResponse();
            }

            if (!await _fileClient.FileExists("formularios", file))
            {
                NotificarErro("Este arquivo já foi removido.");
                return CustomResponse();
            }

            if (!await _fileClient.DeleteFile("formularios", file))
            {
                NotificarErro("Falha ao remover o arquivo.");
                return CustomResponse();
            }

            return CustomResponse();
        }


        [HttpGet]
        [Route("responsaveisRecebimentos")]
        public async Task<IActionResult> ObterResponsaveisRecebimento([FromQuery] PaginationQuery paginationQuery)
        {
            var usuarios = await _authenticationService.GetAllUsersPaginated(page: paginationQuery.PageNumber, pageSize: paginationQuery.PageSize);

            if (!usuarios.Any()) return CustomResponse();

            var responsaveisViewModel = usuarios.Select(u => new
            {
                label = u.UserName,
                value = new { UsuarioId = u.Id, Nome = u.UserName, u.Email }
            }
            ).ToList();

            return CustomResponse(responsaveisViewModel);
        }


        [HttpGet]
        [Route("respostas/{arquivo}/imagem")]
        public async Task<IActionResult> ObterDataAnexo(string arquivo)
        {

            var data = await _fileClient.GetFileArray("formularios", arquivo);

            if (data.Length < 0)
            {
                NotificarErro("Falha ao realizar o download do anexo!");
                return CustomResponse();
            }

            //var newImage = ResizeImage(result, 595, 842);

            return CustomResponse(data);
        }

        [HttpGet("respostas/{arquivo}")]
        [AllowAnonymous]
        public async Task<ActionResult<Stream>> ObterAnexo(string arquivo)
        {
            var result = await _fileClient.GetFileStream("formularios", arquivo);

            if (result.Length < 0)
            {
                NotificarErro("Falha ao realizar o download do anexo!");
                return CustomResponse();
            }

            return result;
        }


        [HttpGet("respostas/{arquivo}/pdf")]
        public async Task<IActionResult> ObterArquivoVisita(string arquivo)
        {

            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Não foi possível baixar o arquivo, tente novamente!");
                return CustomResponse();
            }
            var result = await _fileClient.GetFileStream("formularios", arquivo);

            if (result.Length < 0)
            {
                NotificarErro("Arquivo não encontrado!");
                return CustomResponse();
            }

            return File(result, GetMimeType(arquivo), arquivo);
        }

        private string GetMimeType(string file)
        {
            string extension = Path.GetExtension(file).ToLowerInvariant();
            switch (extension)
            {
                case ".txt": return "text/plain";
                case ".pdf": return "application/pdf";
                case ".doc": return "application/vnd.ms-word";
                case ".docx": return "application/vnd.ms-word";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.ms-excel";
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".gif": return "image/gif";
                case ".csv": return "text/csv";
                case ".zip": return "application/zip";
                default: return "";
            }
        }


        [HttpGet("respostas/{respostaId:guid}/anexos")]
        public async Task<ActionResult<List<string>>> ObterAnexosPorRespostaId(Guid respostaId)
        {

            var resposta = await _formularioRepository.ObterRespostaPorId(respostaId);

            if (resposta == null)
            {
                NotificarErro("Falha ao buscar anexos!");
                return CustomResponse();
            }

            //var itensCaixa = _mapper.Map<List<CaixaItemViewModel>>(await _caixaRepository.ObterItensPorCaixaId(caixaId));
            var anexos = new List<string>();


            foreach (var item in resposta.Anexos)
            {
                anexos.Add(await _fileClient.GetFileArray("formularios", item.Arquivo));
            }

            return CustomResponse(anexos);
        }
    }
}
