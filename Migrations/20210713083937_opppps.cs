using Microsoft.EntityFrameworkCore.Migrations;

namespace DonationManagement.Migrations
{
    public partial class opppps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hosptial",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Hosptial",
                table: "Donors");

            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "Patients",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Hospital",
                table: "Patients",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Hospital",
                table: "Donors",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hospital",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Hospital",
                table: "Donors");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Patients",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "Hosptial",
                table: "Patients",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hosptial",
                table: "Donors",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");
        }
    }
}
