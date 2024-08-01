using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "log",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level = table.Column<int>(type: "integer", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "text", nullable: false),
                    passwordhash = table.Column<string>(name: "password_hash", type: "text", nullable: false),
                    passwordsalt = table.Column<string>(name: "password_salt", type: "text", nullable: false),
                    refkey = table.Column<string>(name: "ref_key", type: "text", nullable: true),
                    refuserid = table.Column<Guid>(name: "ref_user_id", type: "uuid", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    codedate = table.Column<DateTime>(name: "code_date", type: "timestamp with time zone", nullable: true),
                    isblocked = table.Column<bool>(name: "is_blocked", type: "boolean", nullable: false),
                    blockeddate = table.Column<DateTime>(name: "blocked_date", type: "timestamp with time zone", nullable: true),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_user_ref_user_id",
                        column: x => x.refuserid,
                        principalSchema: "public",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_ref_user_id",
                schema: "public",
                table: "user",
                column: "ref_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user",
                schema: "public");
        }
    }
}
