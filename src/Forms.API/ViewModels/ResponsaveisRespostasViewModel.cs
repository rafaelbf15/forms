using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forms.API.ViewModels.Forms
{
    public class ResponsaveisRespostasViewModel
    {
        public Guid ResponsavelResposta { get; set; }
        public List<Guid> FormulariosIds { get; set; }
    }
}
