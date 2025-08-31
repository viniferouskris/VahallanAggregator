using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vahallan_Ingredient_Aggregator.Migrations
{
    /// <inheritdoc />
    public partial class initCreate : Migration
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
                    StoredQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StoredUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServingSize = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPerPackage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CaloriesPerServing = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ServingsPerPackage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsSystemIngredient = table.Column<bool>(type: "bit", nullable: true),
                    IsPromoted = table.Column<bool>(type: "bit", nullable: true),
                    PromotionStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SystemIngredientId = table.Column<int>(type: "int", nullable: true),
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
                    { "1", "b9f616e8-2215-41c4-9251-26ebebf7463f", "Admin", "ADMIN" },
                    { "2", "186722cf-5464-4823-8e90-e2f16b1cb3fd", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-user-id", 0, "ce6241af-70c6-43c1-a54c-c75ec1467c76", "admin@yourapp.com", true, false, null, "ADMIN@YOURAPP.COM", "ADMIN@YOURAPP.COM", "AQAAAAIAAYagAAAAEJftVca1LaVWXTnJErd7QQfo2Jy9ya06M6OOBB4XWhmOYfDyag8ysDv4aHIhSlyfRw==", null, false, "ff2bdfff-e047-455a-b405-3568638a53f6", false, "admin@yourapp.com" });

            migrationBuilder.InsertData(
                table: "BaseIngredientComponent",
                columns: new[] { "Id", "CaloriesPerServing", "CostPerPackage", "CreatedAt", "CreatedById", "IsPromoted", "IsSystemIngredient", "ModifiedAt", "Name", "PromotionEndDate", "PromotionStartDate", "Quantity", "ServingsPerPackage", "StoredQuantity", "StoredUnit", "SystemIngredientId", "Type", "Unit" },
                values: new object[,]
                {
                    { 1, 70m, 5.99m, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9186), "system", false, true, null, "Fresh Mozzarella", null, null, 1m, 16m, 28.35m, "g", null, "Ingredient", "oz" },
                    { 2, 22m, 3.00m, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9289), "system", false, true, null, "Ripe Tomatoes", null, null, 1m, 4m, 1m, "count", null, "Ingredient", "count" },
                    { 3, 1m, 2.99m, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9297), "system", false, true, null, "Fresh Basil Leaves", null, null, 1m, 30m, 1m, "count", null, "Ingredient", "count" },
                    { 4, 120m, 8.99m, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9303), "system", false, true, null, "Extra Virgin Olive Oil", null, null, 1m, 33.8m, 14.7868m, "ml", null, "Ingredient", "tbsp" },
                    { 5, 14m, 5.99m, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9310), "system", false, true, null, "Balsamic Vinegar", null, null, 1m, 16.9m, 14.7868m, "ml", null, "Ingredient", "tbsp" },
                    { 6, 0m, 0.99m, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9318), "system", false, true, null, "Salt", null, null, 1m, 156m, 4.92892m, "ml", null, "Ingredient", "tsp" },
                    { 7, 0m, 3.99m, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9325), "system", false, true, null, "Black Pepper", null, null, 1m, 144m, 4.92892m, "ml", null, "Ingredient", "tsp" }
                });

            migrationBuilder.InsertData(
                table: "BaseIngredientComponent",
                columns: new[] { "Id", "CookTimeMinutes", "CreatedAt", "CreatedById", "Description", "ExternalId", "ExternalSource", "ExternalUrl", "ImportedAt", "Instructions", "IsPublic", "ModifiedAt", "Name", "NumberOfServings", "OriginalRecipeId", "PrepTimeMinutes", "Quantity", "StoredQuantity", "StoredUnit", "Type", "Unit", "Version" },
                values: new object[] { 8, 0, new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9162), "system", "A simple and elegant Italian salad made with fresh mozzarella, tomatoes, and basil.", null, null, null, null, "1. Slice the mozzarella and tomatoes into 1/4-inch thick slices.\n2. On a serving plate, alternately arrange the mozzarella and tomato slices in a circular pattern.\n3. Tuck fresh basil leaves between the mozzarella and tomato slices.\n4. Drizzle with extra virgin olive oil and balsamic vinegar.\n5. Season with salt and freshly ground black pepper.\n6. Serve immediately at room temperature.", true, null, "Classic Caprese Salad", 4m, null, 15, 4m, 4m, "serving", "Recipe", "serving", 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "admin-user-id" });

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

            migrationBuilder.InsertData(
                table: "RecipePhotos",
                columns: new[] { "Id", "ContentType", "Description", "FileName", "FilePath", "FileSize", "IsApproved", "IsMain", "IsMainPhoto", "ModifiedAt", "RecipeId", "ThumbnailPath", "UploadedAt", "UploadedById" },
                values: new object[,]
                {
                    { 1, "image/jpeg", "Classic Caprese Salad with alternating slices of mozzarella and tomato", "caprese-main.jpg", "/recipe-photos/originals/caprese-main.jpg", 19661L, true, true, false, null, 8, "/recipe-photos/thumbnails/caprese-main.jpg", new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9359), "system" },
                    { 2, "image/jpeg", "Caprese Salad from a different angle", "caprese-salad-recipe-1.jpg", "/recipe-photos/originals/caprese-salad-recipe-1.jpg", 18432L, true, false, false, null, 8, "/recipe-photos/thumbnails/caprese-salad-recipe-1.jpg", new DateTime(2025, 8, 31, 0, 41, 27, 834, DateTimeKind.Utc).AddTicks(9364), "system" }
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
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");

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
                name: "RecipeIngredients");

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
