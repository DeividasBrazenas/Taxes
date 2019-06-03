using Microsoft.EntityFrameworkCore.Migrations;

namespace Taxes.Service.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MunicipalityId",
                table: "Tax",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Tax_MunicipalityId",
                table: "Tax",
                column: "MunicipalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tax_Municipality_MunicipalityId",
                table: "Tax",
                column: "MunicipalityId",
                principalTable: "Municipality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tax_Municipality_MunicipalityId",
                table: "Tax");

            migrationBuilder.DropIndex(
                name: "IX_Tax_MunicipalityId",
                table: "Tax");

            migrationBuilder.AlterColumn<int>(
                name: "MunicipalityId",
                table: "Tax",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
