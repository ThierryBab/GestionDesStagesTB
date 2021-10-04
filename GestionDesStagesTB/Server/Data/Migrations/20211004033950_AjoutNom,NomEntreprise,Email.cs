using Microsoft.EntityFrameworkCore.Migrations;

namespace GestionDesStagesTB.Server.Data.Migrations
{
    public partial class AjoutNomNomEntrepriseEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Stage",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Stage",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nomEntreprise",
                table: "Stage",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "nomEntreprise",
                table: "Stage");
        }
    }
}
