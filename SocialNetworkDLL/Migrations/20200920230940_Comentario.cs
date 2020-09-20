using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetworkDLL.Migrations
{
    public partial class Comentario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Usuarios_UsuarioId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_UsuarioId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Comentarios");

            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "Comentarios",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PerfilId",
                table: "Comentarios",
                column: "PerfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Perfis_PerfilId",
                table: "Comentarios",
                column: "PerfilId",
                principalTable: "Perfis",
                principalColumn: "PerfilId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Perfis_PerfilId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_PerfilId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Comentarios");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Comentarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioId",
                table: "Comentarios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Usuarios_UsuarioId",
                table: "Comentarios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
