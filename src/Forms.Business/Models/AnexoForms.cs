using Forms.Core.DomainObjects;
using System;

namespace Forms.Business.Models
{
    public class AnexoForms: Entity
    {
        public Guid RespostaId { get; private set; }
        public string Arquivo { get; private set; }
        public string ThumbnailMedio { get; private set; }
        public string Thumbnail { get; private set; }
        public DateTime? DataUltimaModificacao { get; private set; }
        public DateTime DataCadastro { get; private set; }

        //EF Rel
        public Resposta Resposta { get; set; }


        public AnexoForms(Guid respostaId, string arquivo, DateTime? dataUltimaModificacao, string thumbnailMedio, string thumbnail)
        {
            RespostaId = respostaId;
            Arquivo = arquivo;
            DataUltimaModificacao = dataUltimaModificacao;
            ThumbnailMedio = thumbnailMedio;
            Thumbnail = thumbnail;
        }

        protected AnexoForms() { }
    }
}
