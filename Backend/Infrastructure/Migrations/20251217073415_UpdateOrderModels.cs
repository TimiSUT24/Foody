using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "ShippingInformation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipmentId",
                table: "ShippingInformation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingId",
                table: "ShippingInformation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingUrl",
                table: "ShippingInformation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeight",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightValue",
                table: "OrderItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "ShippingInformation");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "ShippingInformation");

            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "ShippingInformation");

            migrationBuilder.DropColumn(
                name: "TrackingUrl",
                table: "ShippingInformation");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WeightValue",
                table: "OrderItems");
        }
    }
}
