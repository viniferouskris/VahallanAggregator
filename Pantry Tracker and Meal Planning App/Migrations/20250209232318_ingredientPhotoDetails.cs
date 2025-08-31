using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Migrations
{
    /// <inheritdoc />
    public partial class ingredientPhotoDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4884));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4898));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4906));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4913));

            migrationBuilder.UpdateData(
                table: "BaseIngredientComponent",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4608));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 1,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "RecipePhotos",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadedAt",
                value: new DateTime(2025, 2, 9, 23, 23, 17, 694, DateTimeKind.Utc).AddTicks(4935));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
