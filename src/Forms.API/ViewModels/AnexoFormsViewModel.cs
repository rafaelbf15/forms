using Forms.Core.DomainObjects;
using System;

namespace Forms.API.ViewModels.Forms
{
    public class AnexoFormsViewModel: Entity
    {
        public Guid? Id { get; set; }
        public Guid? RespostaId { get;  set; }
        public string Arquivo { get;  set; }
        public string ThumbnailMedio { get;  set; }
        public string Thumbnail { get;  set; }
        public string File { get; set; }
        public DateTime? DataUltimaModificacao { get;  set; }
        public DateTime DataCadastro { get; set; }
    }
}
