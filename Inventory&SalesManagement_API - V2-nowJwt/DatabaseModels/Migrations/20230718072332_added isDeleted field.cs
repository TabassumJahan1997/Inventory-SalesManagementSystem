using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseModels.Migrations
{
    public partial class addedisDeletedfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "tblUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "tblProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "tblProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "tblOrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "tblOrder",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "tblUser");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "tblProduct");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "tblProduct");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "tblOrderDetails");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "tblOrder");
        }
    }
}
