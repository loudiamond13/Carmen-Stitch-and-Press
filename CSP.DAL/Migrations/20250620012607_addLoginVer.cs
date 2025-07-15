using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addLoginVer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CodeSentAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginVerificationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeSentAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LoginVerificationCode",
                table: "AspNetUsers");
        }
    }
}
