using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectC_.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileCedula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CT_Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "CT_Incidents",
                columns: table => new
                {
                    IncidentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReportedByAccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_Incidents", x => x.IncidentId);
                    table.ForeignKey(
                        name: "FK_CT_Incidents_CT_Accounts_ReportedByAccountId",
                        column: x => x.ReportedByAccountId,
                        principalTable: "CT_Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "CT_UserProfiles",
                columns: table => new
                {
                    UserProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_UserProfiles", x => x.UserProfileId);
                    table.ForeignKey(
                        name: "FK_CT_UserProfiles_CT_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "CT_Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CT_Incidents_ReportedByAccountId",
                table: "CT_Incidents",
                column: "ReportedByAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CT_UserProfiles_AccountId",
                table: "CT_UserProfiles",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CT_Incidents");

            migrationBuilder.DropTable(
                name: "CT_UserProfiles");

            migrationBuilder.DropTable(
                name: "CT_Accounts");
        }
    }
}
