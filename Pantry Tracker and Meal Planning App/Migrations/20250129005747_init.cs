using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseIngredientComponent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StoredQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StoredUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: true),
                    CostPerPackage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CaloriesPerServing = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ServingsPerPackage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServingSize = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrepTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    CookTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    OriginalRecipeId = table.Column<int>(type: "int", nullable: true),
                    NumberOfServings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseIngredientComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseIngredientComponent_BaseIngredientComponent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseIngredientComponent_BaseIngredientComponent_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AutoConfirmMealPlanDeductions = table.Column<bool>(type: "bit", nullable: false),
                    DefaultLowStockThreshold = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DefaultReplenishmentDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelatedItemId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PantryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    IngredientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CurrentUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastPurchaseQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LastPurchaseUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastPurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastPurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplenishmentDays = table.Column<int>(type: "int", nullable: true),
                    DailyUsageRate = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LowStockThreshold = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPlanRecipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealPlanId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    PrepareOnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServingSize = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlanRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlanRecipe_BaseIngredientComponent_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPlanRecipe_BaseIngredientComponent_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipePhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMainPhoto = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    UploadedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipePhotos_BaseIngredientComponent_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeTags",
                columns: table => new
                {
                    RecipesId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTags", x => new { x.RecipesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_RecipeTags_BaseIngredientComponent_RecipesId",
                        column: x => x.RecipesId,
                        principalTable: "BaseIngredientComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeTags_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "31ee4e09-e30d-4e53-8a77-b0d422751890", "Admin", "ADMIN" },
                    { "2", "9c4af1b2-806b-4ae8-b6dd-b2ab5e430edf", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-user-id", 0, "cf9a335b-6e16-42ee-a378-cee45852c532", "admin@yourapp.com", true, false, null, "ADMIN@YOURAPP.COM", "ADMIN@YOURAPP.COM", "AQAAAAIAAYagAAAAEHB+AqZii0JZda6EoUj3tXt/3mfJaqWp0fzwGH2ibfjYX/5FMUuPjn+3wLulAmeRQA==", null, false, "dff3cd6d-8653-4a2c-bfd6-dcc5181c0bf8", false, "admin@yourapp.com" });

            migrationBuilder.InsertData(
                table: "BaseIngredientComponent",
                columns: new[] { "Id", "CookTimeMinutes", "CreatedAt", "CreatedById", "Description", "ExternalId", "ExternalSource", "ExternalUrl", "ImportedAt", "Instructions", "IsPublic", "ModifiedAt", "Name", "NumberOfServings", "OriginalRecipeId", "ParentId", "PrepTimeMinutes", "Quantity", "RecipeId", "StoredQuantity", "StoredUnit", "Type", "Unit", "Version" },
                values: new object[] { 8, 0, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3317), "system", "A simple and elegant Italian salad made with fresh mozzarella, tomatoes, and basil.", null, null, null, null, "1. Slice the mozzarella and tomatoes into 1/4-inch thick slices.\r\n2. On a serving plate, alternately arrange the mozzarella and tomato slices in a circular pattern.\r\n3. Tuck fresh basil leaves between the mozzarella and tomato slices.\r\n4. Drizzle with extra virgin olive oil and balsamic vinegar.\r\n5. Season with salt and freshly ground black pepper.\r\n6. Serve immediately at room temperature.", true, null, "Classic Caprese Salad", 0m, null, null, 15, 4m, null, 4m, "serving", "Recipe", "serving", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "admin-user-id" });

            migrationBuilder.InsertData(
                table: "BaseIngredientComponent",
                columns: new[] { "Id", "CaloriesPerServing", "CostPerPackage", "CreatedAt", "CreatedById", "ModifiedAt", "Name", "ParentId", "Quantity", "RecipeId", "ServingsPerPackage", "StoredQuantity", "StoredUnit", "Type", "Unit" },
                values: new object[,]
                {
                    { 1, 70m, 5.99m, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3337), "system", null, "Fresh Mozzarella", 8, 16m, null, 16m, 453.592m, "g", "Ingredient", "oz" },
                    { 2, 22m, 3.00m, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3344), "system", null, "Ripe Tomatoes", 8, 4m, null, 4m, 4m, "count", "Ingredient", "count" },
                    { 3, 1m, 2.99m, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3350), "system", null, "Fresh Basil Leaves", 8, 20m, null, 30m, 20m, "count", "Ingredient", "count" },
                    { 4, 120m, 8.99m, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3355), "system", null, "Extra Virgin Olive Oil", 8, 2m, null, 33.8m, 29.5735m, "ml", "Ingredient", "tbsp" },
                    { 5, 14m, 5.99m, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3360), "system", null, "Balsamic Vinegar", 8, 2m, null, 16.9m, 29.5735m, "ml", "Ingredient", "tbsp" },
                    { 6, 0m, 0.99m, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3366), "system", null, "Salt", 8, 0.5m, null, 156m, 2.46446m, "ml", "Ingredient", "tsp" },
                    { 7, 0m, 3.99m, new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3371), "system", null, "Black Pepper", 8, 0.25m, null, 144m, 1.23223m, "ml", "Ingredient", "tsp" }
                });

            migrationBuilder.InsertData(
                table: "RecipePhotos",
                columns: new[] { "Id", "ContentType", "Description", "FileName", "FilePath", "FileSize", "IsApproved", "IsMain", "IsMainPhoto", "ModifiedAt", "RecipeId", "ThumbnailPath", "UploadedAt", "UploadedById" },
                values: new object[,]
                {
                    { 1, "image/jpeg", "Classic Caprese Salad with alternating slices of mozzarella and tomato", "caprese-main.jpg", "/recipe-photos/originals/caprese-main.jpg", 20L, true, false, true, null, 8, "/recipe-photos/thumbnails/caprese-main.jpg", new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3384), "system" },
                    { 2, "image/jpeg", "Caprese Salad from a different angle", "caprese-salad-recipe-1.jpg", "/recipe-photos/originals/caprese-salad-recipe-1.jpg", 18432L, true, false, false, null, 8, "/recipe-photos/thumbnails/caprese-salad-recipe-1.jpg", new DateTime(2025, 1, 29, 0, 57, 46, 780, DateTimeKind.Utc).AddTicks(3388), "system" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseIngredientComponent_ParentId",
                table: "BaseIngredientComponent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseIngredientComponent_RecipeId",
                table: "BaseIngredientComponent",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanRecipe_MealPlanId_RecipeId_PrepareOnDate",
                table: "MealPlanRecipe",
                columns: new[] { "MealPlanId", "RecipeId", "PrepareOnDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanRecipe_RecipeId",
                table: "MealPlanRecipe",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_UserId",
                table: "NotificationPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PantryItems_ExpirationDate",
                table: "PantryItems",
                column: "ExpirationDate");

            migrationBuilder.CreateIndex(
                name: "IX_RecipePhotos_RecipeId",
                table: "RecipePhotos",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_TagsId",
                table: "RecipeTags",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "MealPlanRecipe");

            migrationBuilder.DropTable(
                name: "NotificationPreferences");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PantryItems");

            migrationBuilder.DropTable(
                name: "RecipePhotos");

            migrationBuilder.DropTable(
                name: "RecipeTags");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BaseIngredientComponent");

            migrationBuilder.DropTable(
                name: "Tag");
        }
    }
}
