using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordsAndTenants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_CurrentWeatherCalls_CurrentWeatherId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_FiveDaysWeatherCalls_FiveDaysWeatherId",
                table: "Records");

            migrationBuilder.AlterColumn<Guid>(
                name: "FiveDaysWeatherId",
                table: "Records",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentWeatherId",
                table: "Records",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Records",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "FiveDaysWeatherCalls",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "CurrentWeatherCalls",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Records_TenantId",
                table: "Records",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FiveDaysWeatherCalls_TenantId",
                table: "FiveDaysWeatherCalls",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentWeatherCalls_TenantId",
                table: "CurrentWeatherCalls",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWeatherCalls_Tenants_TenantId",
                table: "CurrentWeatherCalls",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FiveDaysWeatherCalls_Tenants_TenantId",
                table: "FiveDaysWeatherCalls",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_CurrentWeatherCalls_CurrentWeatherId",
                table: "Records",
                column: "CurrentWeatherId",
                principalTable: "CurrentWeatherCalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_FiveDaysWeatherCalls_FiveDaysWeatherId",
                table: "Records",
                column: "FiveDaysWeatherId",
                principalTable: "FiveDaysWeatherCalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Tenants_TenantId",
                table: "Records",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWeatherCalls_Tenants_TenantId",
                table: "CurrentWeatherCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_FiveDaysWeatherCalls_Tenants_TenantId",
                table: "FiveDaysWeatherCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_CurrentWeatherCalls_CurrentWeatherId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_FiveDaysWeatherCalls_FiveDaysWeatherId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Tenants_TenantId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_TenantId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_FiveDaysWeatherCalls_TenantId",
                table: "FiveDaysWeatherCalls");

            migrationBuilder.DropIndex(
                name: "IX_CurrentWeatherCalls_TenantId",
                table: "CurrentWeatherCalls");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "FiveDaysWeatherCalls");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CurrentWeatherCalls");

            migrationBuilder.AlterColumn<Guid>(
                name: "FiveDaysWeatherId",
                table: "Records",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentWeatherId",
                table: "Records",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_CurrentWeatherCalls_CurrentWeatherId",
                table: "Records",
                column: "CurrentWeatherId",
                principalTable: "CurrentWeatherCalls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_FiveDaysWeatherCalls_FiveDaysWeatherId",
                table: "Records",
                column: "FiveDaysWeatherId",
                principalTable: "FiveDaysWeatherCalls",
                principalColumn: "Id");
        }
    }
}
