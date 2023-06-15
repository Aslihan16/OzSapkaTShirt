using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzSapkaTShirt.Migrations
{
    public partial class citys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Citys_CityCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<byte>(
                name: "CityCode",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Citys_CityCode",
                table: "AspNetUsers",
                column: "CityCode",
                principalTable: "Citys",
                principalColumn: "PlateCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Citys_CityCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<byte>(
                name: "CityCode",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Citys_CityCode",
                table: "AspNetUsers",
                column: "CityCode",
                principalTable: "Citys",
                principalColumn: "PlateCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
