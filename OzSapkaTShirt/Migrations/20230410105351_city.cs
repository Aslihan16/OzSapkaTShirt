using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzSapkaTShirt.Migrations
{
    public partial class city : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "CityCode",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "Citys",
                columns: table => new
                {
                    PlateCode = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citys", x => x.PlateCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CityCode",
                table: "AspNetUsers",
                column: "CityCode");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Citys_CityCode",
                table: "AspNetUsers",
                column: "CityCode",
                principalTable: "Citys",
                principalColumn: "PlateCode",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Citys_CityCode",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Citys");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CityCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CityCode",
                table: "AspNetUsers");
        }
    }
}
