using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Identity.Api.Migrations
{
    public partial class InitialMigration : Migration
    {
        #region Instance Members

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Addresses", table => new
            {
                Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                StreetAddress = table.Column<string>("nvarchar(max)", nullable: false),
                ApartmentNumber = table.Column<string>("nvarchar(max)", nullable: true),
                Zipcode = table.Column<string>("nvarchar(10)", maxLength: 10, nullable: false),
                State = table.Column<string>("nvarchar(max)", nullable: false),
                City = table.Column<string>("nvarchar(max)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
            });

            migrationBuilder.CreateTable("Contacts", table => new
            {
                Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>("nvarchar(max)", nullable: false),
                LastName = table.Column<string>("nvarchar(max)", nullable: false),
                Phone = table.Column<string>("nvarchar(max)", nullable: false),
                Email = table.Column<string>("nvarchar(max)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Contacts", x => x.Id);
            });

            migrationBuilder.CreateTable("IdentityProviders",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Scheme = table.Column<string>("nvarchar(200)", nullable: false),
                    DisplayName = table.Column<string>("nvarchar(200)", nullable: true),
                    Enabled = table.Column<bool>("bit", nullable: false),
                    Type = table.Column<string>("nvarchar(20)", nullable: false),
                    Properties = table.Column<string>("nvarchar(max)", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityProviders", x => x.Id);
                });

            migrationBuilder.CreateTable("PersonalDetails",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    LastFourOfSocial = table.Column<string>("nvarchar(4)", maxLength: 4, nullable: false),
                    DateOfBirth = table.Column<DateTime>("datetime2", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.Id);
                });

            migrationBuilder.CreateTable("RoleClaims", table => new
            {
                Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaims", x => x.Id);
            });

            migrationBuilder.CreateTable("Roles", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                Name = table.Column<string>("nvarchar(max)", nullable: true),
                NormalizedName = table.Column<string>("nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

            migrationBuilder.CreateTable("UserClaims", table => new
            {
                Id = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UserClaims", x => x.Id);
            });

            migrationBuilder.CreateTable("UserLogins", table => new
            {
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                LoginProvider = table.Column<string>("nvarchar(max)", nullable: true),
                ProviderKey = table.Column<string>("nvarchar(max)", nullable: true),
                ProviderDisplayName = table.Column<string>("nvarchar(max)", nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UserLogins", x => x.UserId);
            });

            migrationBuilder.CreateTable("UserRoles", table => new
            {
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                RoleId = table.Column<string>("nvarchar(450)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new
                {
                    x.UserId,
                    x.RoleId
                });
            });

            migrationBuilder.CreateTable("UserTokens", table => new
            {
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                LoginProvider = table.Column<string>("nvarchar(max)", nullable: true),
                Name = table.Column<string>("nvarchar(max)", nullable: true),
                Value = table.Column<string>("nvarchar(max)", nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UserTokens", x => x.UserId);
            });

            migrationBuilder.CreateTable("UserIdentities", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                AddressId = table.Column<int>("int", nullable: false),
                ContactInfoId = table.Column<int>("int", nullable: false),
                PersonalInfoId = table.Column<int>("int", nullable: false),
                UserName = table.Column<string>("nvarchar(max)", nullable: true),
                NormalizedUserName = table.Column<string>("nvarchar(max)", nullable: true),
                Email = table.Column<string>("nvarchar(max)", nullable: true),
                NormalizedEmail = table.Column<string>("nvarchar(max)", nullable: true),
                EmailConfirmed = table.Column<bool>("bit", nullable: false),
                PasswordHash = table.Column<string>("nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>("nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>("bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>("bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>("bit", nullable: false),
                AccessFailedCount = table.Column<int>("int", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UserIdentities", x => x.Id);
                table.ForeignKey("FK_UserIdentities_Addresses_AddressId", x => x.AddressId, "Addresses", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_UserIdentities_Contacts_ContactInfoId", x => x.ContactInfoId, "Contacts", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_UserIdentities_PersonalDetails_PersonalInfoId", x => x.PersonalInfoId, "PersonalDetails", "Id",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex("IX_UserIdentities_AddressId", "UserIdentities", "AddressId");

            migrationBuilder.CreateIndex("IX_UserIdentities_ContactInfoId", "UserIdentities", "ContactInfoId");

            migrationBuilder.CreateIndex("IX_UserIdentities_PersonalInfoId", "UserIdentities", "PersonalInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("IdentityProviders");

            migrationBuilder.DropTable("RoleClaims");

            migrationBuilder.DropTable("Roles");

            migrationBuilder.DropTable("UserClaims");

            migrationBuilder.DropTable("UserIdentities");

            migrationBuilder.DropTable("UserLogins");

            migrationBuilder.DropTable("UserRoles");

            migrationBuilder.DropTable("UserTokens");

            migrationBuilder.DropTable("Addresses");

            migrationBuilder.DropTable("Contacts");

            migrationBuilder.DropTable("PersonalDetails");
        }

        #endregion
    }
}