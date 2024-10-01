using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.API.RESTful.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IDRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IDRol);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IDUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFirst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameLast = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<int>(type: "int", nullable: false),
                    Birth = table.Column<DateOnly>(type: "date", nullable: false),
                    IDRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IDUser);
                    table.ForeignKey(
                        name: "FK_Users_Roles_IDRol",
                        column: x => x.IDRol,
                        principalTable: "Roles",
                        principalColumn: "IDRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IDRol",
                table: "Users",
                column: "IDRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
