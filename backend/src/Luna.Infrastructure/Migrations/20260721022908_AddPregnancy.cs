using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPregnancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pregnancies_Users_UserId",
                table: "Pregnancies");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Pregnancies",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Pregnancies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Pregnancies",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Pregnancies",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PregnancyCount",
                table: "Pregnancies",
                newName: "pregnancy_count");

            migrationBuilder.RenameColumn(
                name: "LastMenstrualPeriod",
                table: "Pregnancies",
                newName: "last_menstrual_period");

            migrationBuilder.RenameColumn(
                name: "IsFirstPregnancy",
                table: "Pregnancies",
                newName: "is_first_pregnancy");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Pregnancies",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "EstimatedDueDate",
                table: "Pregnancies",
                newName: "estimated_due_date");

            migrationBuilder.RenameColumn(
                name: "EndedAt",
                table: "Pregnancies",
                newName: "ended_at");

            migrationBuilder.RenameColumn(
                name: "CurrentWeek",
                table: "Pregnancies",
                newName: "current_week");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Pregnancies",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Pregnancies_UserId",
                table: "Pregnancies",
                newName: "IX_Pregnancies_user_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "Pregnancies",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "Pregnancies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "pregnancy_count",
                table: "Pregnancies",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "Pregnancies",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "Pregnancies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Pregnancies_is_active",
                table: "Pregnancies",
                column: "is_active");

            migrationBuilder.AddForeignKey(
                name: "FK_Pregnancies_Users_user_id",
                table: "Pregnancies",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pregnancies_Users_user_id",
                table: "Pregnancies");

            migrationBuilder.DropIndex(
                name: "IX_Pregnancies_is_active",
                table: "Pregnancies");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Pregnancies",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Pregnancies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Pregnancies",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Pregnancies",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "pregnancy_count",
                table: "Pregnancies",
                newName: "PregnancyCount");

            migrationBuilder.RenameColumn(
                name: "last_menstrual_period",
                table: "Pregnancies",
                newName: "LastMenstrualPeriod");

            migrationBuilder.RenameColumn(
                name: "is_first_pregnancy",
                table: "Pregnancies",
                newName: "IsFirstPregnancy");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Pregnancies",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "estimated_due_date",
                table: "Pregnancies",
                newName: "EstimatedDueDate");

            migrationBuilder.RenameColumn(
                name: "ended_at",
                table: "Pregnancies",
                newName: "EndedAt");

            migrationBuilder.RenameColumn(
                name: "current_week",
                table: "Pregnancies",
                newName: "CurrentWeek");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Pregnancies",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Pregnancies_user_id",
                table: "Pregnancies",
                newName: "IX_Pregnancies_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Pregnancies",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pregnancies",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<int>(
                name: "PregnancyCount",
                table: "Pregnancies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Pregnancies",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Pregnancies",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddForeignKey(
                name: "FK_Pregnancies_Users_UserId",
                table: "Pregnancies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
