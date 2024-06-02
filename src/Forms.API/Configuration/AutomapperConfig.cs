using AutoMapper;
using Forms.API.ViewModels.Forms;
using Forms.Business.DTO;
using Forms.Business.Models;
using Forms.Core.Data.Filters;
using Forms.Core.Data.Queries;
using System;
using System.Linq;

namespace Forms.API.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            #region Pagination
            CreateMap<PaginationFilter, PaginationQuery>().ReverseMap();
            #endregion


            #region Forms

            CreateMap<Formulario, FormularioFormsViewModel>()
                .ForMember(dest => dest.Perguntas, opt => opt.MapFrom(src => src.Perguntas));

            CreateMap<FormularioFormsViewModel, Formulario>();



            CreateMap<AnexoFormsViewModel, AnexoForms>().ReverseMap();

            CreateMap<PerguntaFormsViewModel, Pergunta>().ReverseMap();


            CreateMap<Resposta, RespostaFormsViewModel>()
                .ForMember(dest => dest.Anexos, opt => opt.MapFrom(src => src.Anexos));

            CreateMap<RespostaFormsViewModel, Resposta>();

            CreateMap<ResponsavelRecebimentoFormsViewModel, ResponsavelRecebimento>().ReverseMap();

            CreateMap<ResponsaveisRespostasViewModel, ResponsaveisRespostas>().ReverseMap();

            #endregion
        }
    }
}
