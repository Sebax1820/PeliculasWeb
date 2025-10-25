using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasWeb.Migrations
{
    /// <inheritdoc />
    public partial class NuevoAtributo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagenUrl",
                table: "Peliculas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imagenUrl",
                table: "Peliculas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
