using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Migrations
{
    public partial class AddBuyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "buyerseq",
                schema: "ordering",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "paymentseq",
                schema: "ordering",
                incrementBy: 10);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                schema: "ordering",
                table: "orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                schema: "ordering",
                table: "orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "buyers",
                schema: "ordering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    IdentityGuid = table.Column<string>(maxLength: 200, nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cardtypes",
                schema: "ordering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cardtypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "paymentmethods",
                schema: "ordering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CardTypeId = table.Column<int>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    Alias = table.Column<string>(maxLength: 200, nullable: false),
                    CardHolderName = table.Column<string>(maxLength: 200, nullable: false),
                    CardNumber = table.Column<string>(maxLength: 25, nullable: false),
                    Expiration = table.Column<DateTime>(maxLength: 25, nullable: false),
                    SecurityNumber = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentmethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_paymentmethods_buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalSchema: "ordering",
                        principalTable: "buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paymentmethods_cardtypes_CardTypeId",
                        column: x => x.CardTypeId,
                        principalSchema: "ordering",
                        principalTable: "cardtypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_BuyerId",
                schema: "ordering",
                table: "orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_PaymentMethodId",
                schema: "ordering",
                table: "orders",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_buyers_IdentityGuid",
                schema: "ordering",
                table: "buyers",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_BuyerId",
                schema: "ordering",
                table: "paymentmethods",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_CardTypeId",
                schema: "ordering",
                table: "paymentmethods",
                column: "CardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_buyers_BuyerId",
                schema: "ordering",
                table: "orders",
                column: "BuyerId",
                principalSchema: "ordering",
                principalTable: "buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_paymentmethods_PaymentMethodId",
                schema: "ordering",
                table: "orders",
                column: "PaymentMethodId",
                principalSchema: "ordering",
                principalTable: "paymentmethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_buyers_BuyerId",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_paymentmethods_PaymentMethodId",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropTable(
                name: "paymentmethods",
                schema: "ordering");

            migrationBuilder.DropTable(
                name: "buyers",
                schema: "ordering");

            migrationBuilder.DropTable(
                name: "cardtypes",
                schema: "ordering");

            migrationBuilder.DropIndex(
                name: "IX_orders_BuyerId",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_PaymentMethodId",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropSequence(
                name: "buyerseq",
                schema: "ordering");

            migrationBuilder.DropSequence(
                name: "paymentseq",
                schema: "ordering");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                schema: "ordering",
                table: "orders");
        }
    }
}
