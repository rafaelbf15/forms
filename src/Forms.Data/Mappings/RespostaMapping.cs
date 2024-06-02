using Forms.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Data.Mappings
{
    public class RespostaMapping : IEntityTypeConfiguration<Resposta>
    {
        public void Configure(EntityTypeBuilder<Resposta> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasMany(r => r.Anexos)
               .WithOne(a => a.Resposta)
               .HasForeignKey(a => a.RespostaId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.Texto)
                .HasColumnType("varchar(2000)");

            builder.ToTable("forms_respostas");
                
        }
    }
}
