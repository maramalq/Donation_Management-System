using Microsoft.EntityFrameworkCore.Migrations;

namespace DonationManagement.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donors_Admin_AdminId",
                table: "Donors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Admin_AdminId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_AdminId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Donors_AdminId",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Donors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Donors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AdminId",
                table: "Patients",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Donors_AdminId",
                table: "Donors",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donors_Admin_AdminId",
                table: "Donors",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Admin_AdminId",
                table: "Patients",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
