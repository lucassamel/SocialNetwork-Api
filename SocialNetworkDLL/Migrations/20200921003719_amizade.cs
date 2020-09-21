using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetworkDLL.Migrations
{
    public partial class amizade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Perfis_UserId",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "Amigo",
                table: "Amizades");

            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "Amizades");

            migrationBuilder.AddColumn<int>(
                name: "PerfilAmigoId",
                table: "Amizades",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PerfilUsuarioId",
                table: "Amizades",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_UserId",
                table: "Perfis",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amizades_PerfilAmigoId",
                table: "Amizades",
                column: "PerfilAmigoId");

            migrationBuilder.CreateIndex(
                name: "IX_Amizades_PerfilUsuarioId",
                table: "Amizades",
                column: "PerfilUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Amizades_Perfis_PerfilAmigoId",
                table: "Amizades",
                column: "PerfilAmigoId",
                principalTable: "Perfis",
                principalColumn: "PerfilId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Amizades_Perfis_PerfilUsuarioId",
                table: "Amizades",
                column: "PerfilUsuarioId",
                principalTable: "Perfis",
                principalColumn: "PerfilId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amizades_Perfis_PerfilAmigoId",
                table: "Amizades");

            migrationBuilder.DropForeignKey(
                name: "FK_Amizades_Perfis_PerfilUsuarioId",
                table: "Amizades");

            migrationBuilder.DropIndex(
                name: "IX_Perfis_UserId",
                table: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_Amizades_PerfilAmigoId",
                table: "Amizades");

            migrationBuilder.DropIndex(
                name: "IX_Amizades_PerfilUsuarioId",
                table: "Amizades");

            migrationBuilder.DropColumn(
                name: "PerfilAmigoId",
                table: "Amizades");

            migrationBuilder.DropColumn(
                name: "PerfilUsuarioId",
                table: "Amizades");

            migrationBuilder.AddColumn<int>(
                name: "Amigo",
                table: "Amizades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Usuario",
                table: "Amizades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_UserId",
                table: "Perfis",
                column: "UserId");
        }
    }
}
