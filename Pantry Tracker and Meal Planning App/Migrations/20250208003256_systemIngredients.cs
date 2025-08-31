using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Migrations
{
    /// <inheritdoc />
    public partial class systemIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPromoted",
                table: "BaseIngredientComponent",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemIngredient",
                table: "BaseIngredientComponent",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotionEndDate",
                table: "BaseIngredientComponent",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotionStartDate",
                table: "BaseIngredientComponent",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SystemIngredientId",
                table: "BaseIngredientComponent",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "9cadacaa-7a05-4df3-a431-5edab582d7ef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "526bf264-e1e5-4646-bc1c-5e2439cf11d7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "589a4996-1a5a-414a-93e1-6b5ee1f3fc4f", "AQAAAAIAAYagAAAAEA9OxZq3rB0aUJEq9I/3o7jjW6EQxXy47cIQazEFVhEpTRDmBXakaeVMlNLNbwWpYw==", "90482457-dc93-46c8-977a-e385e56ee514" });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsPromoted", "IsSystemIngredient", "PromotionEndDate", "PromotionStartDate", "SystemIngredientId" },
                values: new object[] { new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5667), false, true, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "IsPromoted", "IsSystemIngredient", "PromotionEndDate", "PromotionStartDate", "SystemIngredientId" },
                values: new object[] { new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5674), false, true, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "IsPromoted", "IsSystemIngredient", "PromotionEndDate", "PromotionStartDate", "SystemIngredientId" },
                values: new object[] { new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5678), false, true, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "IsPromoted", "IsSystemIngredient", "PromotionEndDate", "PromotionStartDate", "SystemIngredientId" },
                values: new object[] { new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5682), false, true, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "IsPromoted", "IsSystemIngredient", "PromotionEndDate", "PromotionStartDate", "SystemIngredientId" },
                values: new object[] { new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5685), false, true, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "IsPromoted", "IsSystemIngredient", "PromotionEndDate", "PromotionStartDate", "SystemIngredientId" },
                values: new object[] { new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5689), false, true, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "IsPromoted", "IsSystemIngredient", "PromotionEndDate", "PromotionStartDate", "SystemIngredientId" },
                values: new object[] { new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5692), false, true, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5646));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 1,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5702));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5704));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPromoted",
                table: "BaseIngredientComponent");

            migrationBuilder.DropColumn(
                name: "IsSystemIngredient",
                table: "BaseIngredientComponent");

            migrationBuilder.DropColumn(
                name: "PromotionEndDate",
                table: "BaseIngredientComponent");

            migrationBuilder.DropColumn(
                name: "PromotionStartDate",
                table: "BaseIngredientComponent");

            migrationBuilder.DropColumn(
                name: "SystemIngredientId",
                table: "BaseIngredientComponent");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "31ee4e09-e30d-4e53-8a77-b0d422751890");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "9c4af1b2-806b-4ae8-b6dd-b2ab5e430edf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf9a335b-6e16-42ee-a378-cee45852c532", "AQAAAAIAAYagAAAAEHB+AqZii0JZda6EoUj3tXt/3mfJaqWp0fzwGH2ibfjYX/5FMUuPjn+3wLulAmeRQA==", "dff3cd6d-8653-4a2c-bfd6-dcc5181c0bf8" });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3337));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3344));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3350));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3355));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3360));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3366));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3371));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3317));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 1,
                column: "UploadedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3384));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadedAt",
                value: new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3388));
        }
    }
}
