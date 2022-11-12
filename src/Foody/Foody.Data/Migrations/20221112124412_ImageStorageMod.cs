using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Foody.Data.Migrations
{
    public partial class ImageStorageMod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_items_app_file_image_id",
                schema: "api",
                table: "items");

            migrationBuilder.DropTable(
                name: "app_file",
                schema: "api");

            migrationBuilder.DropIndex(
                name: "ix_items_image_id",
                schema: "api",
                table: "items");

            migrationBuilder.DropColumn(
                name: "image_id",
                schema: "api",
                table: "items");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "api",
                table: "items",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.CreateTable(
                name: "images",
                schema: "api",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<byte[]>(type: "bytea", maxLength: 2097152, nullable: false),
                    untrusted_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    file_extension = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    item_id = table.Column<int>(type: "integer", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    added_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_items_item_id",
                        column: x => x.item_id,
                        principalSchema: "api",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Table holding all images in database.");

            migrationBuilder.CreateIndex(
                name: "ix_images_item_id",
                schema: "api",
                table: "images",
                column: "item_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "images",
                schema: "api");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "api",
                table: "items",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "image_id",
                schema: "api",
                table: "items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "app_file",
                schema: "api",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    added_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    content = table.Column<byte[]>(type: "bytea", maxLength: 1048576, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    file_extension = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<decimal>(type: "numeric", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    untrusted_name = table.Column<string>(type: "text", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_file", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_items_image_id",
                schema: "api",
                table: "items",
                column: "image_id");

            migrationBuilder.AddForeignKey(
                name: "fk_items_app_file_image_id",
                schema: "api",
                table: "items",
                column: "image_id",
                principalSchema: "api",
                principalTable: "app_file",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
