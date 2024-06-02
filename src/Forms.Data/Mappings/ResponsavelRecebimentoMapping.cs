using Forms.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Data.Mappings
{
    public class ResponsavelRecebimentoMapping : IEntityTypeConfiguration<ResponsavelRecebimento>
    {
        public void Configure(EntityTypeBuilder<ResponsavelRecebimento> builder)
        {
            builder.HasKey(rp => rp.Id);

            builder.ToTable("forms_responsaveis_recebimentos");
        }
    }
}
