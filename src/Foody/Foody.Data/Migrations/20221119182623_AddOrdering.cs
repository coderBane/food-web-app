using System;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Foody.Data.Migrations
{
    public partial class AddOrdering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:status", "preparing,en_route,delivered,cancelled");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "api",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_no = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tel = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    delivery_address_line1 = table.Column<string>(type: "text", nullable: false),
                    delivery_address_line2 = table.Column<string>(type: "text", nullable: true),
                    delivery_address_city = table.Column<string>(type: "text", nullable: false),
                    delivery_address_post_code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    delivery_address_country = table.Column<string>(type: "text", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<Status>(type: "api.status", nullable: false),
                    added_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders_details",
                schema: "api",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_details_items_product_id",
                        column: x => x.product_id,
                        principalSchema: "api",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_orders_details_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "api",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "OrderNumberIndex",
                schema: "api",
                table: "orders",
                column: "order_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_orders_details_order_id",
                schema: "api",
                table: "orders_details",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_details_product_id",
                schema: "api",
                table: "orders_details",
                column: "product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders_details",
                schema: "api");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "api");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:status", "preparing,en_route,delivered,cancelled");
        }
    }
}
