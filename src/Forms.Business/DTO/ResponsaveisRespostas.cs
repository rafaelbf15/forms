using Forms.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Business.DTO
{
    public class ResponsaveisRespostas
    {
        public Guid ResponsavelResposta { get; set; }
        public List<Guid> FormulariosIds { get; set; }
    }
}
