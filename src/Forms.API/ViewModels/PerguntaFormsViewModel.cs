using System;
using System.Collections.Generic;

namespace Forms.API.ViewModels.Forms
{
    public class PerguntaFormsViewModel
    {
        public Guid? Id { get; set; }
        public Guid? FormularioId { get;  set; }
        public string Titulo { get;  set; }
        public bool AnexoObrigatorio { get;  set; }
        public bool TextoObrigatorio { get;  set; }
        public IEnumerable<RespostaFormsViewModel> Respostas { get;  set; }
    }
}
