using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YourBudget.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Login = table.Column<string>(maxLength: 100, nullable: false),
                    PassHash = table.Column<string>(maxLength: 200, nullable: true),
                    ResetToken = table.Column<string>(maxLength: 100, nullable: true),
                    Mail = table.Column<string>(maxLength: 100, nullable: true),
                    Language = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Balance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Сumulative = table.Column<bool>(nullable: false),
                    Planned = table.Column<double>(nullable: false),
                    Debet = table.Column<double>(nullable: false),
                    Credit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetItems_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Allowed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetUsers_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActorId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    BudgetItemId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Discription = table.Column<string>(nullable: true),
                    Category = table.Column<string>(maxLength: 250, nullable: true),
                    Commited = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Users_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_BudgetId",
                table: "BudgetItems",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_OwnerId",
                table: "Budgets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsers_BudgetId",
                table: "BudgetUsers",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetUsers_UserId",
                table: "BudgetUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ActorId",
                table: "Operations",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_BudgetItemId",
                table: "Operations",
                column: "BudgetItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetUsers");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
