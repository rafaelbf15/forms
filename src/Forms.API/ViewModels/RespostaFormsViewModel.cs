using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forms.API.ViewModels.Forms
{
    public class RespostaFormsViewModel
    {
        public Guid? Id { get; set; }
        public Guid? PerguntaId { get;  set; }
        public string Texto { get;  set; }
        public DateTime DataPreenchimento { get;  set; }
        public Guid? ResponsavelCadastro { get;  set; }
        public IEnumerable<AnexoFormsViewModel> Anexos { get;  set; }
        public Guid? IdFormulario { get; set; }

    }
}
