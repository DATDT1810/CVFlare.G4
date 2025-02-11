using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CV_Flare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CvSubmissions_JobDescriptions_JobDescId",
                table: "CvSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTemplates_JobDescriptions_JobDescId",
                table: "UserTemplates");

            migrationBuilder.DropTable(
                name: "JobDescriptions");

            migrationBuilder.DropTable(
                name: "ServiceRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserTemplates_JobDescId",
                table: "UserTemplates");

            migrationBuilder.DropIndex(
                name: "IX_CvSubmissions_JobDescId",
                table: "CvSubmissions");

            migrationBuilder.DropColumn(
                name: "JobDescId",
                table: "UserTemplates");

            migrationBuilder.DropColumn(
                name: "JobDescId",
                table: "CvSubmissions");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JobDescripion",
                table: "CvSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "JobDescripion",
                table: "CvSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "JobDescId",
                table: "UserTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobDescId",
                table: "CvSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobDescriptions",
                columns: table => new
                {
                    JobDescId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptions", x => x.JobDescId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRatings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RatingIcon = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRatings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_ServiceRatings_Packages_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTemplates_JobDescId",
                table: "UserTemplates",
                column: "JobDescId");

            migrationBuilder.CreateIndex(
                name: "IX_CvSubmissions_JobDescId",
                table: "CvSubmissions",
                column: "JobDescId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRatings_ServiceId",
                table: "ServiceRatings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRatings_UserId",
                table: "ServiceRatings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CvSubmissions_JobDescriptions_JobDescId",
                table: "CvSubmissions",
                column: "JobDescId",
                principalTable: "JobDescriptions",
                principalColumn: "JobDescId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTemplates_JobDescriptions_JobDescId",
                table: "UserTemplates",
                column: "JobDescId",
                principalTable: "JobDescriptions",
                principalColumn: "JobDescId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
