﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore_DataAccess.Migrations
{
    public partial class AddApplicationTypeToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ApplicationTypeId",
                table: "Products",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ApplicationTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "Products");
        }
    }
}
