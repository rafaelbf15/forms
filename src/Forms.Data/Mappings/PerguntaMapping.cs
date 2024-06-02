using Forms.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Data.Mappings
{
    public class PerguntaMapping : IEntityTypeConfiguration<Pergunta>
    {
        public void Configure(EntityTypeBuilder<Pergunta> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Respostas)
                 .WithOne(r => r.Pergunta)
                 .HasForeignKey(r => r.PerguntaId);

            builder.Property(p => p.Titulo)
                .HasColumnType("varchar(2000)");


            builder.ToTable("forms_perguntas");
        }
    }
}
