using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Migrations
{
    /// <inheritdoc />
    public partial class ingredientPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "76872303-6f18-43e2-8df3-72980f76446f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "ac8874d6-d489-4509-9785-62c8426e2a55");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54c2d12d-799c-4334-9829-05b24b2e720a", "AQAAAAIAAYagAAAAECBX7mD9QOKabzLSkntdSFUsyGppRFVp31n+YGn17lF0LTwogxqXTP7WQlcYc6vHNg==", "f14c2096-a132-4440-9e2b-d43fea5f3925" });

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2175));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2196));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2214));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2224));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2430));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2152));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 1,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2449));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 9, 22, 12, 29, 432, DateTimeKind.Utc).AddTicks(2455));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5667));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5678));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5682));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5685));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5689));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 8, 0, 32, 56, 203, DateTimeKind.Utc).AddTicks(5692));

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
    }
}
