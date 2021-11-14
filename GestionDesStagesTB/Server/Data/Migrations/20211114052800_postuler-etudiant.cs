using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestionDesStagesTB.Server.Data.Migrations
{
    public partial class postuleretudiant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entreprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NomEntreprise = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NomResponsable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PrenomResponsable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PosteTelephonique = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entreprise", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stage_Id",
                table: "Stage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Entreprise_Id",
                table: "Stage",
                column: "Id",
                principalTable: "Entreprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Entreprise_Id",
                table: "Stage");

            migrationBuilder.DropTable(
                name: "Entreprise");

            migrationBuilder.DropIndex(
                name: "IX_Stage_Id",
                table: "Stage");
        }
    }
}
