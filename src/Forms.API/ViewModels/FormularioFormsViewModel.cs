using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.API.ViewModels.Forms
{
    public class FormularioFormsViewModel
    {
        public Guid? Id { get; set; }
        public string Titulo { get;  set; }
        public string Descricao { get; set; }
        public Guid ResponsavelCadastro { get; set; }
        public bool VisualizacaoTodos { get; set; }
        public IEnumerable<PerguntaFormsViewModel> Perguntas { get;  set; }
        public IEnumerable<ResponsavelRecebimentoFormsViewModel> ResponsaveisRecebimento { get;  set; }
    }
}
