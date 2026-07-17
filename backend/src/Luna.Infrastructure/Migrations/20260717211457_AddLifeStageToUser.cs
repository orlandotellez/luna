using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLifeStageToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "EstimatedDueDate",
                table: "Users",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastMenstrualPeriod",
                table: "Users",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LifeStage",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedDueDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastMenstrualPeriod",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LifeStage",
                table: "Users");
        }
    }
}
