using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectC_.Migrations
{
    /// <inheritdoc />
    public partial class ArregloCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "CT_Incidents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "CT_Incidents");
        }
    }
}
