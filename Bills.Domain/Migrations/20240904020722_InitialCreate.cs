using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bills.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_en = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name_br = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    symbol = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    document = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    currency = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Currency",
                        column: x => x.currency,
                        principalTable: "Currency",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    billsName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    dueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    installments = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<decimal>(type: "money", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bills_Users",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "InstallmentBills",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    billsId = table.Column<long>(type: "bigint", nullable: true),
                    installment_number = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<decimal>(type: "money", nullable: true),
                    dueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallmentBills", x => x.id);
                    table.ForeignKey(
                        name: "FK_InstallmentBills_Bills",
                        column: x => x.billsId,
                        principalTable: "Bills",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_InstallmentBills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_userId",
                table: "Bills",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentBills_billsId",
                table: "InstallmentBills",
                column: "billsId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentBills_UserId",
                table: "InstallmentBills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_currency",
                table: "Users",
                column: "currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstallmentBills");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
