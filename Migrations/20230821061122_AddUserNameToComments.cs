using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anonymous_Topics.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ParentTopicCommentId",
                table: "TopicComments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TopicComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TopicComments");

            migrationBuilder.AlterColumn<int>(
                name: "ParentTopicCommentId",
                table: "TopicComments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
