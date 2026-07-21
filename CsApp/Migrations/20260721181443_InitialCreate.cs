using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CsApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Files",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    filename = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_file = table.Column<int>(type: "integer", nullable: false),
                    delta_date = table.Column<TimeSpan>(type: "interval", nullable: false),
                    min_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    avg_execution_time = table.Column<decimal>(type: "numeric", nullable: false),
                    avg_value = table.Column<decimal>(type: "numeric", nullable: false),
                    median_value = table.Column<decimal>(type: "numeric", nullable: false),
                    max_value = table.Column<decimal>(type: "numeric", nullable: false),
                    min_value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_file = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    execution_time = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_filename",
                schema: "public",
                table: "Files",
                column: "filename",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Results",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Values",
                schema: "public");
        }
    }
}
