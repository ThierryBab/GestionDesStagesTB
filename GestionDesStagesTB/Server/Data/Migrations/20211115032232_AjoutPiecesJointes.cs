using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestionDesStagesTB.Server.Data.Migrations
{
    public partial class AjoutPiecesJointes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PieceJointe",
                columns: table => new
                {
                    PieceJointeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DateVersee = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PieceJointe", x => x.PieceJointeId);
                    table.ForeignKey(
                        name: "FK_PieceJointe_Etudiant_Id",
                        column: x => x.Id,
                        principalTable: "Etudiant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PieceJointe_Id",
                table: "PieceJointe",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PieceJointe");
        }
    }
}
