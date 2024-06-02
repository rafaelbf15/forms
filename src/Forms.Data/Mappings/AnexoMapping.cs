using Forms.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Data.Mappings
{
    public class AnexoMapping : IEntityTypeConfiguration<AnexoForms>
    {
        public void Configure(EntityTypeBuilder<AnexoForms> builder)
        {
            builder.HasKey(a => a.Id);

            builder.ToTable("forms_anexos");
        }
    }
}
