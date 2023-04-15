using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSubscriber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscribed",
                table: "Subscribers");

            migrationBuilder.AddColumn<int>(
                name: "SubscribeStatus",
                table: "Subscribers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscribeStatus",
                table: "Subscribers");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribed",
                table: "Subscribers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
