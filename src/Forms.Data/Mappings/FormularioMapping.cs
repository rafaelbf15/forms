using Forms.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Data.Mappings
{
    public class FormularioMapping : IEntityTypeConfiguration<Formulario>
    {
        public void Configure(EntityTypeBuilder<Formulario> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasMany(f => f.Perguntas)
                 .WithOne(p => p.Formulario)
                 .HasForeignKey(p => p.FormularioId);

            builder.HasMany(f => f.ResponsaveisRecebimento)
                .WithOne(p => p.Formulario)
                .HasForeignKey(p => p.FormularioId);

            builder.Property(f => f.Titulo)
               .HasColumnType("varchar(200)");

            builder.Property(f => f.Descricao)
               .HasColumnType("varchar(2000)");


            builder.ToTable("forms_formularios");
        }
    }
}
