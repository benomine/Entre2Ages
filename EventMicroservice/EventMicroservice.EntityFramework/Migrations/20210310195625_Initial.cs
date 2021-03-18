using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventMicroservice.EntityFramework.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<string>(type: "text", nullable: true),
                    guest = table.Column<string>(type: "text", nullable: true),
                    epoch_start = table.Column<long>(type: "bigint", nullable: false),
                    epoch_end = table.Column<long>(type: "bigint", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
