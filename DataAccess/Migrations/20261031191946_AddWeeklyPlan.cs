using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklyPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "BreakfastIngredients",
                newName: "Unit");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "BreakfastIngredients",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "WeeklyPlanItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    BreakfastId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyPlanItems_Breakfasts_BreakfastId",
                        column: x => x.BreakfastId,
                        principalTable: "Breakfasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyPlanItems_BreakfastId",
                table: "WeeklyPlanItems",
                column: "BreakfastId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyPlanItems_Username_Day",
                table: "WeeklyPlanItems",
                columns: new[] { "Username", "Day" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyPlanItems");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BreakfastIngredients");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "BreakfastIngredients",
                newName: "Quantity");
        }
    }
}
