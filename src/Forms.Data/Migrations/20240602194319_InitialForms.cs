using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forms.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "forms_formularios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(2000)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataExclusao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResponsavelCadastro = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResponsavelAlteracao = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResponsavelExclusao = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    VisualizacaoTodos = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forms_formularios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "forms_perguntas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FormularioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "varchar(2000)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataExclusao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResponsavelCadastro = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResponsavelAlteracao = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResponsavelExclusao = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnexoObrigatorio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TextoObrigatorio = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forms_perguntas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_forms_perguntas_forms_formularios_FormularioId",
                        column: x => x.FormularioId,
                        principalTable: "forms_formularios",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "forms_responsaveis_recebimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FormularioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(400)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(400)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forms_responsaveis_recebimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_forms_responsaveis_recebimentos_forms_formularios_Formulario~",
                        column: x => x.FormularioId,
                        principalTable: "forms_formularios",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "forms_respostas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PerguntaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Texto = table.Column<string>(type: "varchar(2000)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataPreenchimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResponsavelCadastro = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdFormulario = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forms_respostas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_forms_respostas_forms_perguntas_PerguntaId",
                        column: x => x.PerguntaId,
                        principalTable: "forms_perguntas",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "forms_anexos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RespostaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Arquivo = table.Column<string>(type: "varchar(400)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThumbnailMedio = table.Column<string>(type: "varchar(400)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Thumbnail = table.Column<string>(type: "varchar(400)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataUltimaModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forms_anexos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_forms_anexos_forms_respostas_RespostaId",
                        column: x => x.RespostaId,
                        principalTable: "forms_respostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_forms_anexos_RespostaId",
                table: "forms_anexos",
                column: "RespostaId");

            migrationBuilder.CreateIndex(
                name: "IX_forms_perguntas_FormularioId",
                table: "forms_perguntas",
                column: "FormularioId");

            migrationBuilder.CreateIndex(
                name: "IX_forms_responsaveis_recebimentos_FormularioId",
                table: "forms_responsaveis_recebimentos",
                column: "FormularioId");

            migrationBuilder.CreateIndex(
                name: "IX_forms_respostas_PerguntaId",
                table: "forms_respostas",
                column: "PerguntaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forms_anexos");

            migrationBuilder.DropTable(
                name: "forms_responsaveis_recebimentos");

            migrationBuilder.DropTable(
                name: "forms_respostas");

            migrationBuilder.DropTable(
                name: "forms_perguntas");

            migrationBuilder.DropTable(
                name: "forms_formularios");
        }
    }
}
