using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classifications");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "RawMaterials");

            migrationBuilder.DropColumn(
                name: "OfferValidUntil",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "Calculation",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "EuroFIRCode",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "MatrixUnit",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "MatrixUnitCode",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "MethodIndicator",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "MethodIndicatorCode",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "MethodType",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "MethodTypeCode",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "OriginCode",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "Publication",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeCode",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "ValueTypeCode",
                table: "NutritionValues");

            migrationBuilder.DropColumn(
                name: "WeightGram",
                table: "NutritionValues");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "Products",
                newName: "WeightUnit");

            migrationBuilder.RenameColumn(
                name: "ScientificName",
                table: "Products",
                newName: "WeightText");

            migrationBuilder.RenameColumn(
                name: "Project",
                table: "Products",
                newName: "ProductInformation");

            migrationBuilder.RenameColumn(
                name: "Origin",
                table: "Products",
                newName: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "FoodTypeId",
                table: "Products",
                newName: "SubSubCategoryId");

            migrationBuilder.RenameColumn(
                name: "FoodType",
                table: "Products",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "DiscountInfo",
                table: "Products",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "CookingMethod",
                table: "Products",
                newName: "ComparePrice");

            migrationBuilder.RenameColumn(
                name: "Analysis",
                table: "Products",
                newName: "Ca");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "MainCategory");

            migrationBuilder.AddColumn<List<string>>(
                name: "Attributes",
                table: "Products",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightValue",
                table: "Products",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "NutritionValues",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: true),
                    FoodId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributes_Products_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubSubCategories_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubSubCategoryId",
                table: "Products",
                column: "SubSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_FoodId",
                table: "ProductAttributes",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubSubCategories_SubCategoryId",
                table: "SubSubCategories",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubSubCategories_SubSubCategoryId",
                table: "Products",
                column: "SubSubCategoryId",
                principalTable: "SubSubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubSubCategories_SubSubCategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductAttributes");

            migrationBuilder.DropTable(
                name: "SubSubCategories");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubSubCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Attributes",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WeightValue",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "WeightUnit",
                table: "Products",
                newName: "Supplier");

            migrationBuilder.RenameColumn(
                name: "WeightText",
                table: "Products",
                newName: "ScientificName");

            migrationBuilder.RenameColumn(
                name: "SubSubCategoryId",
                table: "Products",
                newName: "FoodTypeId");

            migrationBuilder.RenameColumn(
                name: "ProductInformation",
                table: "Products",
                newName: "Project");

            migrationBuilder.RenameColumn(
                name: "Ingredients",
                table: "Products",
                newName: "Origin");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Products",
                newName: "FoodType");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Products",
                newName: "DiscountInfo");

            migrationBuilder.RenameColumn(
                name: "ComparePrice",
                table: "Products",
                newName: "CookingMethod");

            migrationBuilder.RenameColumn(
                name: "Ca",
                table: "Products",
                newName: "Analysis");

            migrationBuilder.RenameColumn(
                name: "MainCategory",
                table: "Categories",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "OfferValidUntil",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "NutritionValues",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Calculation",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EuroFIRCode",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatrixUnit",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatrixUnitCode",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodIndicator",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodIndicatorCode",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodType",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodTypeCode",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginCode",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Publication",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceType",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeCode",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueType",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueTypeCode",
                table: "NutritionValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightGram",
                table: "NutritionValues",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FoodId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Facet = table.Column<string>(type: "text", nullable: true),
                    FacetCode = table.Column<string>(type: "text", nullable: true),
                    LangualId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classifications_Products_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FoodId = table.Column<int>(type: "integer", nullable: false),
                    CookingFactor = table.Column<string>(type: "text", nullable: false),
                    FatFactor = table.Column<decimal>(type: "numeric", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RetentionFactorsJson = table.Column<string>(type: "text", nullable: true),
                    WaterFactor = table.Column<decimal>(type: "numeric", nullable: true),
                    WeightAfterCooking = table.Column<decimal>(type: "numeric", nullable: true),
                    WeightBeforeCooking = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_Products_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RawMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FoodId = table.Column<int>(type: "integer", nullable: false),
                    ConvertedToRaw = table.Column<decimal>(type: "numeric", nullable: true),
                    Cooking = table.Column<string>(type: "text", nullable: true),
                    Factor = table.Column<decimal>(type: "numeric", nullable: true),
                    FoodEx2 = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Portion = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawMaterials_Products_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_FoodId",
                table: "Classifications",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_FoodId",
                table: "Ingredients",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterials_FoodId",
                table: "RawMaterials",
                column: "FoodId");
        }
    }
}
