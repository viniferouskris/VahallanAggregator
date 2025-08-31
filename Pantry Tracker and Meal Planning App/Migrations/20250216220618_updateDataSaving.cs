using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Migrations
{
    /// <inheritdoc />
    public partial class updateDataSaving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseIngredientComponent_BaseIngredientComponent_ParentId",
                table: "BaseIngredientComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseIngredientComponent_BaseIngredientComponent_RecipeId",
                table: "BaseIngredientComponent");

            migrationBuilder.DropIndex(
                name: "IX_BaseIngredientComponent_ParentId",
                table: "BaseIngredientComponent");

            migrationBuilder.DropIndex(
                name: "IX_BaseIngredientComponent_RecipeId",
                table: "BaseIngredientComponent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "BaseIngredientComponent");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "BaseIngredientComponent");

            migrationBuilder.AlterColumn<string>(
                name: "StoredUnit",
                table: "BaseIngredientComponent",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<decimal>(
                name: "StoredQuantity",
                table: "BaseIngredientComponent",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_BaseIngredientComponent_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_BaseIngredientComponent_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "540a7f91-4b50-484c-baff-580f6e87d5ef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "b120677a-6390-493c-b1eb-d51bf571364a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "21cd6f44-be80-4452-ad80-f73bd548dd86", "AQAAAAIAAYagAAAAEKbsyA2GPkWVVYUFB/uPrmoemcr3QguNaQg1qh6Nq+/VAFVj1GXfby5aL8sQJu5sgQ==", "bd2d7f12-9f40-46dc-b370-518027f25847" });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Quantity", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3570), 1m, 28.35m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Quantity", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3577), 1m, 1m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Quantity", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3583), 1m, 1m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Quantity", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3588), 1m, 14.7868m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Quantity", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3593), 1m, 14.7868m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Quantity", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3599), 1m, 4.92892m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Quantity", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3603), 1m, 4.92892m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "NumberOfServings" },
                values: new object[] { new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3546), 4m });

            migrationBuilder.InsertData(
                table: "RecipeIngredients",
                columns: new[] { "Id", "IngredientId", "Quantity", "RecipeId", "Unit" },
                values: new object[,]
                {
                    { 1, 1, 16m, 8, "oz" },
                    { 2, 2, 4m, 8, "count" },
                    { 3, 3, 20m, 8, "count" },
                    { 4, 4, 2m, 8, "tbsp" },
                    { 5, 5, 2m, 8, "tbsp" },
                    { 6, 6, 0.5m, 8, "tsp" },
                    { 7, 7, 0.25m, 8, "tsp" }
                });

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FileSize", "IsMain", "IsMainPhoto", "UploadedAt" },
                values: new object[] { 19661L, true, false, new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3631) });

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 16, 22, 6, 17, 337, DateTimeKind.Utc).AddTicks(3635));

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.AlterColumn<string>(
                name: "StoredUnit",
                table: "BaseIngredientComponent",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "StoredQuantity",
                table: "BaseIngredientComponent",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "BaseIngredientComponent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "BaseIngredientComponent",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "4bfcdecf-f518-46bd-97cd-c4ba2f348a0b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "cf55bfdb-349f-4cc8-84be-a36829c2756a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f2c580b3-09f3-4471-a852-bac2bdfc46d1", "AQAAAAIAAYagAAAAEDeolEj/IMd+lx0KWqjw1IdRPzYSCuHXyQXAF1JEIlU3INdSGHzh94nQu7HcStVK2g==", "14b2f218-5e47-493d-bce8-f103718b24a4" });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ParentId", "Quantity", "RecipeId", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4864), 8, 16m, null, 453.592m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ParentId", "Quantity", "RecipeId", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4876), 8, 4m, null, 4m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ParentId", "Quantity", "RecipeId", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4884), 8, 20m, null, 20m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ParentId", "Quantity", "RecipeId", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4891), 8, 2m, null, 29.5735m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "ParentId", "Quantity", "RecipeId", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4898), 8, 2m, null, 29.5735m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "ParentId", "Quantity", "RecipeId", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4906), 8, 0.5m, null, 2.46446m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "ParentId", "Quantity", "RecipeId", "StoredQuantity" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4913), 8, 0.25m, null, 1.23223m });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "NumberOfServings", "ParentId", "RecipeId" },
                values: new object[] { new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4608), 0m, null, null });

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FileSize", "IsMain", "IsMainPhoto", "UploadedAt" },
                values: new object[] { 20L, false, true, new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4929) });

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4935));

            migrationBuilder.CreateIndex(
                name: "IX_BaseIngredientComponent_ParentId",
                table: "BaseIngredientComponent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseIngredientComponent_RecipeId",
                table: "BaseIngredientComponent",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseIngredientComponent_BaseIngredientComponent_ParentId",
                table: "BaseIngredientComponent",
                column: "ParentId",
                principalTable: "BaseIngredientComponent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseIngredientComponent_BaseIngredientComponent_RecipeId",
                table: "BaseIngredientComponent",
                column: "RecipeId",
                principalTable: "BaseIngredientComponent",
                principalColumn: "Id");
        }
    }
}
