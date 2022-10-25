using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Accounts.Persistence.Sql.Migrations
{
    public partial class InitialMigration : Migration
    {
        #region Instance Members

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Addresses", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                State = table.Column<string>("nvarchar(max)", nullable: false),
                Zipcode = table.Column<string>("nvarchar(max)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
            });

            migrationBuilder.CreateTable("BusinessInfos", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                BusinessType = table.Column<string>("nvarchar(max)", nullable: false),
                MerchantCategoryCode = table.Column<ushort>("int", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_BusinessInfos", x => x.Id);
            });

            migrationBuilder.CreateTable("BusinessTypes", table => new {Value = table.Column<string>("nvarchar(450)", nullable: false)}, constraints: table =>
            {
                table.PrimaryKey("PK_BusinessTypes", x => x.Value);
            });

            migrationBuilder.CreateTable("ConfirmationCodes", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                SentDate = table.Column<DateTime>("datetime2", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_ConfirmationCodes", x => x.Id);
            });

            migrationBuilder.CreateTable("Contacts", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                Email = table.Column<string>("nvarchar(max)", nullable: false),
                FirstName = table.Column<string>("nvarchar(max)", nullable: false),
                LastName = table.Column<string>("nvarchar(max)", nullable: false),
                Phone = table.Column<string>("nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateTable("MerchantCategoryCodes",
                table => new
                {
                    Value = table.Column<int>("int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("nvarchar(max)", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantCategoryCodes", x => x.Value);
                });

            migrationBuilder.CreateTable("MerchantRegistrationStatuses", table => new {Value = table.Column<string>("nvarchar(450)", nullable: false)},
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantRegistrationStatuses", x => x.Value);
                });

            migrationBuilder.CreateTable("Passwords", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                CreatedOn = table.Column<DateTime>("datetime2", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Passwords", x => x.Id);
            });

            migrationBuilder.CreateTable("PersonalDetails", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
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

            migrationBuilder.CreateTable("States", table => new {Value = table.Column<string>("nvarchar(450)", nullable: false)}, constraints: table =>
            {
                table.PrimaryKey("PK_States", x => x.Value);
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

            migrationBuilder.CreateTable("UserRegistrationStatuses", table => new {Value = table.Column<string>("nvarchar(450)", nullable: false)},
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegistrationStatuses", x => x.Value);
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

            migrationBuilder.CreateTable("MerchantRegistrations", table => new
            {
                _Id = table.Column<string>("nvarchar(450)", nullable: false),
                _AddressId = table.Column<string>("nvarchar(450)", nullable: true),
                _BusinessInfoId = table.Column<string>("nvarchar(450)", nullable: true),
                CompanyName = table.Column<string>("nvarchar(max)", nullable: true),
                RegistrationDate = table.Column<DateTime>("datetime2", nullable: false),
                Status = table.Column<string>("nvarchar(max)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_MerchantRegistrations", x => x._Id);
                table.ForeignKey("FK_MerchantRegistrations_Addresses__AddressId", x => x._AddressId, "Addresses", "Id");
                table.ForeignKey("FK_MerchantRegistrations_BusinessInfos__BusinessInfoId", x => x._BusinessInfoId, "BusinessInfos", "Id");
            });

            migrationBuilder.CreateTable("Merchants", table => new
            {
                _Id = table.Column<string>("nvarchar(450)", nullable: false),
                _AddressId = table.Column<string>("nvarchar(450)", nullable: true),
                _BusinessInfoId = table.Column<string>("nvarchar(450)", nullable: true),
                CompanyName = table.Column<string>("nvarchar(max)", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Merchants", x => x._Id);
                table.ForeignKey("FK_Merchants_Addresses__AddressId", x => x._AddressId, "Addresses", "Id");
                table.ForeignKey("FK_Merchants_BusinessInfos__BusinessInfoId", x => x._BusinessInfoId, "BusinessInfos", "Id");
            });

            migrationBuilder.CreateTable("UserRegistrations", table => new
            {
                _Id = table.Column<string>("nvarchar(450)", nullable: false),
                _AddressId = table.Column<string>("nvarchar(450)", nullable: true),
                _ContactId = table.Column<string>("nvarchar(450)", nullable: true),
                _EmailConfirmationId = table.Column<string>("nvarchar(450)", nullable: true),
                HasEmailBeenVerified = table.Column<bool>("bit", nullable: false),
                HasPhoneBeenVerified = table.Column<bool>("bit", nullable: false),
                HashedPassword = table.Column<string>("nvarchar(max)", nullable: false),
                _PersonalDetailId = table.Column<string>("nvarchar(450)", nullable: true),
                RegistrationDate = table.Column<DateTime>("datetime2", nullable: false),
                _SmsConfirmationId = table.Column<string>("nvarchar(450)", nullable: true),
                Status = table.Column<string>("nvarchar(max)", nullable: false),
                Username = table.Column<string>("nvarchar(max)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UserRegistrations", x => x._Id);
                table.ForeignKey("FK_UserRegistrations_Addresses__AddressId", x => x._AddressId, "Addresses", "Id");
                table.ForeignKey("FK_UserRegistrations_ConfirmationCodes__EmailConfirmationId", x => x._EmailConfirmationId, "ConfirmationCodes", "Id");
                table.ForeignKey("FK_UserRegistrations_ConfirmationCodes__SmsConfirmationId", x => x._SmsConfirmationId, "ConfirmationCodes", "Id");
                table.ForeignKey("FK_UserRegistrations_Contacts__ContactId", x => x._ContactId, "Contacts", "Id");
                table.ForeignKey("FK_UserRegistrations_PersonalDetails__PersonalDetailId", x => x._PersonalDetailId, "PersonalDetails", "Id");
            });

            migrationBuilder.CreateTable("Users", table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                ContactId = table.Column<string>("nvarchar(450)", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                AddressId = table.Column<string>("nvarchar(450)", nullable: true),
                MerchantId = table.Column<string>("nvarchar(max)", nullable: false),
                PasswordId = table.Column<string>("nvarchar(450)", nullable: true),
                PersonalDetailId = table.Column<string>("nvarchar(450)", nullable: true),
                TerminalId = table.Column<string>("nvarchar(max)", nullable: false),
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
                table.PrimaryKey("PK_Users", x => x.Id);
                table.ForeignKey("FK_Users_Addresses_AddressId", x => x.AddressId, "Addresses", "Id");
                table.ForeignKey("FK_Users_Contacts_ContactId", x => x.ContactId, "Contacts", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_Users_Passwords_PasswordId", x => x.PasswordId, "Passwords", "Id");
                table.ForeignKey("FK_Users_PersonalDetails_PersonalDetailId", x => x.PersonalDetailId, "PersonalDetails", "Id");
            });

            migrationBuilder.InsertData("BusinessTypes", "Value",
                new object[] {"Exempt", "LimitedLiability", "NonProfit", "Partnership", "SoleProprietorship"});

            migrationBuilder.InsertData("MerchantCategoryCodes", new[] {"Value", "Name"},
                new object[,] {{4214, "Delivery"}, {7392, "Consulting"}, {8111, "LegalServices"}, {8351, "Childcare"}, {8931, "Accounting"}});

            migrationBuilder.InsertData("MerchantRegistrationStatuses", "Value", new object[] {"Approved", "Expired", "Rejected", "WaitingForRiskAnalysis"});

            migrationBuilder.InsertData("Roles", new[] {"Id", "ConcurrencyStamp", "Name", "NormalizedName"},
                new object[,]
                {
                    {"Administrator", "2a575df4-6a72-4353-a7db-ec3cc214c592", "Administrator", "ADMINISTRATOR"},
                    {"SalesAssociate", "d06b300f-cb32-4340-b556-4c80cf5c1e97", "SalesAssociate", "SALESASSOCIATE"},
                    {"SuperAdmin", "358af4cf-0237-4ae8-bad3-d4f0d99bf0ce", "SuperAdmin", "SUPERADMIN"}
                });

            migrationBuilder.InsertData("States", "Value",
                new object[]
                {
                    "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "DistrictOfColumbia", "Florida",
                    "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine",
                    "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi"
                });

            migrationBuilder.InsertData("States", "Value",
                new object[]
                {
                    "Missouri", "Montana", "Nebraska", "Nevada", "NewHampshire", "NewJersey", "NewMexico", "NewYork", "NorthCarolina", "NorthDakota",
                    "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "RhodeIsland", "SouthCarolina", "SouthDakota", "Tennessee", "Texas", "Utah",
                    "Vermont", "Virginia", "Washington", "WestVirginia", "Wisconsin", "Wyoming"
                });

            migrationBuilder.InsertData("UserRegistrationStatuses", "Value",
                new object[] {"Approved", "Expired", "Rejected", "WaitingForEmailVerification", "WaitingForRiskAnalysis", "WaitingForSmsVerification"});

            migrationBuilder.CreateIndex("IX_MerchantRegistrations__AddressId", "MerchantRegistrations", "_AddressId");

            migrationBuilder.CreateIndex("IX_MerchantRegistrations__BusinessInfoId", "MerchantRegistrations", "_BusinessInfoId");

            migrationBuilder.CreateIndex("IX_Merchants__AddressId", "Merchants", "_AddressId");

            migrationBuilder.CreateIndex("IX_Merchants__BusinessInfoId", "Merchants", "_BusinessInfoId");

            migrationBuilder.CreateIndex("IX_UserRegistrations__AddressId", "UserRegistrations", "_AddressId");

            migrationBuilder.CreateIndex("IX_UserRegistrations__ContactId", "UserRegistrations", "_ContactId");

            migrationBuilder.CreateIndex("IX_UserRegistrations__EmailConfirmationId", "UserRegistrations", "_EmailConfirmationId");

            migrationBuilder.CreateIndex("IX_UserRegistrations__PersonalDetailId", "UserRegistrations", "_PersonalDetailId");

            migrationBuilder.CreateIndex("IX_UserRegistrations__SmsConfirmationId", "UserRegistrations", "_SmsConfirmationId");

            migrationBuilder.CreateIndex("IX_Users_AddressId", "Users", "AddressId");

            migrationBuilder.CreateIndex("IX_Users_ContactId", "Users", "ContactId");

            migrationBuilder.CreateIndex("IX_Users_PasswordId", "Users", "PasswordId");

            migrationBuilder.CreateIndex("IX_Users_PersonalDetailId", "Users", "PersonalDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("BusinessTypes");

            migrationBuilder.DropTable("IdentityProviders");

            migrationBuilder.DropTable("MerchantCategoryCodes");

            migrationBuilder.DropTable("MerchantRegistrations");

            migrationBuilder.DropTable("MerchantRegistrationStatuses");

            migrationBuilder.DropTable("Merchants");

            migrationBuilder.DropTable("RoleClaims");

            migrationBuilder.DropTable("Roles");

            migrationBuilder.DropTable("States");

            migrationBuilder.DropTable("UserClaims");

            migrationBuilder.DropTable("UserLogins");

            migrationBuilder.DropTable("UserRegistrations");

            migrationBuilder.DropTable("UserRegistrationStatuses");

            migrationBuilder.DropTable("UserRoles");

            migrationBuilder.DropTable("Users");

            migrationBuilder.DropTable("UserTokens");

            migrationBuilder.DropTable("BusinessInfos");

            migrationBuilder.DropTable("ConfirmationCodes");

            migrationBuilder.DropTable("Addresses");

            migrationBuilder.DropTable("Contacts");

            migrationBuilder.DropTable("Passwords");

            migrationBuilder.DropTable("PersonalDetails");
        }

        #endregion
    }
}