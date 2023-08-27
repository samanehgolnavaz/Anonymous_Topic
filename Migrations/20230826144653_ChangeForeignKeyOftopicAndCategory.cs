using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anonymous_Topics.Migrations
{
    /// <inheritdoc />
    public partial class ChangeForeignKeyOftopicAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SecurityKey",
                table: "Topics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TopicCategoryId",
                table: "Topics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TopicCategoryId",
                table: "Topics",
                column: "TopicCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_TopicCategories_TopicCategoryId",
                table: "Topics",
                column: "TopicCategoryId",
                principalTable: "TopicCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_TopicCategories_TopicCategoryId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_TopicCategoryId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "SecurityKey",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TopicCategoryId",
                table: "Topics");
        }
    }
}
