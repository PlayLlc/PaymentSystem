using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _DeleteMe.Migrations
{
    public partial class InitialMigration : Migration
    {
        #region Instance Members

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("RoleClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>("nvarchar(max)", nullable: true), ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable("Roles",
                table => new
                {
                    Id = table.Column<string>("nvarchar(450)", nullable: false), Name = table.Column<string>("nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>("nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable("UserClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>("nvarchar(max)", nullable: true), ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable("UserIdentities",
                table => new
                {
                    Id = table.Column<string>("nvarchar(450)", nullable: false), FirstName = table.Column<string>("nvarchar(max)", nullable: false),
                    LastName = table.Column<string>("nvarchar(max)", nullable: false), UserName = table.Column<string>("nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>("nvarchar(max)", nullable: true),
                    Email = table.Column<string>("nvarchar(max)", nullable: true), NormalizedEmail = table.Column<string>("nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>("bit", nullable: false), PasswordHash = table.Column<string>("nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>("nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>("nvarchar(max)", nullable: true), PhoneNumberConfirmed = table.Column<bool>("bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>("bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>("bit", nullable: false), AccessFailedCount = table.Column<int>("int", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentities", x => x.Id);
                });

            migrationBuilder.CreateTable("UserLogins",
                table => new
                {
                    UserId = table.Column<string>("nvarchar(450)", nullable: false), LoginProvider = table.Column<string>("nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>("nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>("nvarchar(max)", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable("UserRoles",
                table => new {UserId = table.Column<string>("nvarchar(450)", nullable: false), RoleId = table.Column<string>("nvarchar(450)", nullable: false)},
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new {x.UserId, x.RoleId});
                });

            migrationBuilder.CreateTable("UserTokens",
                table => new
                {
                    UserId = table.Column<string>("nvarchar(450)", nullable: false), LoginProvider = table.Column<string>("nvarchar(max)", nullable: true),
                    Name = table.Column<string>("nvarchar(max)", nullable: true), Value = table.Column<string>("nvarchar(max)", nullable: true)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("RoleClaims");

            migrationBuilder.DropTable("Roles");

            migrationBuilder.DropTable("UserClaims");

            migrationBuilder.DropTable("UserIdentities");

            migrationBuilder.DropTable("UserLogins");

            migrationBuilder.DropTable("UserRoles");

            migrationBuilder.DropTable("UserTokens");
        }

        #endregion
    }
}