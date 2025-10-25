using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasWeb.Migrations
{
    /// <inheritdoc />
    public partial class AtributoImagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenRuta",
                table: "Peliculas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenRuta",
                table: "Peliculas");
        }
    }
}
