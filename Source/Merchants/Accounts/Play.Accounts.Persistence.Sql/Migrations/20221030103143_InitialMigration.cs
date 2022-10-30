using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Accounts.Persistence.Sql.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BusinessType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MerchantCategoryCode = table.Column<ushort>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessTypes",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTypes", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmationCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scheme = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MerchantCategoryCodes",
                columns: table => new
                {
                    Value = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantCategoryCodes", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "MerchantRegistrationStatuses",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantRegistrationStatuses", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Passwords",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => new { x.UserId, x.CreatedOn });
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastFourOfSocial = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRegistrationStatuses",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegistrationStatuses", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "MerchantRegistrations",
                columns: table => new
                {
                    _Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _AddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    _BusinessInfoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantRegistrations", x => x._Id);
                    table.ForeignKey(
                        name: "FK_MerchantRegistrations_Addresses__AddressId",
                        column: x => x._AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MerchantRegistrations_BusinessInfos__BusinessInfoId",
                        column: x => x._BusinessInfoId,
                        principalTable: "BusinessInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    _Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _AddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    _BusinessInfoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x._Id);
                    table.ForeignKey(
                        name: "FK_Merchants_Addresses__AddressId",
                        column: x => x._AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Merchants_BusinessInfos__BusinessInfoId",
                        column: x => x._BusinessInfoId,
                        principalTable: "BusinessInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRegistrations",
                columns: table => new
                {
                    _Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _AddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    _ContactId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    _EmailConfirmationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HasEmailBeenVerified = table.Column<bool>(type: "bit", nullable: false),
                    HasPhoneBeenVerified = table.Column<bool>(type: "bit", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _PersonalDetailId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _SmsConfirmationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegistrations", x => x._Id);
                    table.ForeignKey(
                        name: "FK_UserRegistrations_Addresses__AddressId",
                        column: x => x._AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRegistrations_ConfirmationCodes__EmailConfirmationId",
                        column: x => x._EmailConfirmationId,
                        principalTable: "ConfirmationCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRegistrations_ConfirmationCodes__SmsConfirmationId",
                        column: x => x._SmsConfirmationId,
                        principalTable: "ConfirmationCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRegistrations_Contacts__ContactId",
                        column: x => x._ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRegistrations_PersonalDetails__PersonalDetailId",
                        column: x => x._PersonalDetailId,
                        principalTable: "PersonalDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PersonalDetailId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TerminalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Passwords_PasswordId_PasswordCreatedOn",
                        columns: x => new { x.PasswordId, x.PasswordCreatedOn },
                        principalTable: "Passwords",
                        principalColumns: new[] { "UserId", "CreatedOn" });
                    table.ForeignKey(
                        name: "FK_Users_PersonalDetails_PersonalDetailId",
                        column: x => x.PersonalDetailId,
                        principalTable: "PersonalDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "BusinessTypes",
                column: "Value",
                values: new object[]
                {
                    "Exempt",
                    "LimitedLiability",
                    "NonProfit",
                    "Partnership",
                    "SoleProprietorship"
                });

            migrationBuilder.InsertData(
                table: "MerchantCategoryCodes",
                columns: new[] { "Value", "Name" },
                values: new object[,]
                {
                    { 4214, "Delivery" },
                    { 7392, "Consulting" },
                    { 8111, "LegalServices" },
                    { 8351, "Childcare" },
                    { 8931, "Accounting" }
                });

            migrationBuilder.InsertData(
                table: "MerchantRegistrationStatuses",
                column: "Value",
                values: new object[]
                {
                    "Approved",
                    "Expired",
                    "Rejected",
                    "WaitingForRiskAnalysis"
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "Administrator", "aaa433d1-cb4c-4d8e-be9d-453e56145758", "Administrator", "ADMINISTRATOR" },
                    { "SalesAssociate", "51e30897-4096-47cf-a65e-0f580908ef64", "SalesAssociate", "SALESASSOCIATE" },
                    { "SuperAdmin", "045784f8-0708-461c-b511-de280bf5c4d5", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "States",
                column: "Value",
                values: new object[]
                {
                    "Alabama",
                    "Alaska",
                    "Arizona",
                    "Arkansas",
                    "California",
                    "Colorado",
                    "Connecticut",
                    "Delaware",
                    "DistrictOfColumbia",
                    "Florida",
                    "Georgia",
                    "Hawaii",
                    "Idaho",
                    "Illinois",
                    "Indiana",
                    "Iowa",
                    "Kansas",
                    "Kentucky",
                    "Louisiana",
                    "Maine",
                    "Maryland",
                    "Massachusetts",
                    "Michigan",
                    "Minnesota",
                    "Mississippi"
                });

            migrationBuilder.InsertData(
                table: "States",
                column: "Value",
                values: new object[]
                {
                    "Missouri",
                    "Montana",
                    "Nebraska",
                    "Nevada",
                    "NewHampshire",
                    "NewJersey",
                    "NewMexico",
                    "NewYork",
                    "NorthCarolina",
                    "NorthDakota",
                    "Ohio",
                    "Oklahoma",
                    "Oregon",
                    "Pennsylvania",
                    "RhodeIsland",
                    "SouthCarolina",
                    "SouthDakota",
                    "Tennessee",
                    "Texas",
                    "Utah",
                    "Vermont",
                    "Virginia",
                    "Washington",
                    "WestVirginia",
                    "Wisconsin",
                    "Wyoming"
                });

            migrationBuilder.InsertData(
                table: "UserRegistrationStatuses",
                column: "Value",
                values: new object[]
                {
                    "Approved",
                    "Expired",
                    "Rejected",
                    "WaitingForEmailVerification",
                    "WaitingForRiskAnalysis",
                    "WaitingForSmsVerification"
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantRegistrations__AddressId",
                table: "MerchantRegistrations",
                column: "_AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantRegistrations__BusinessInfoId",
                table: "MerchantRegistrations",
                column: "_BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants__AddressId",
                table: "Merchants",
                column: "_AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants__BusinessInfoId",
                table: "Merchants",
                column: "_BusinessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrations__AddressId",
                table: "UserRegistrations",
                column: "_AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrations__ContactId",
                table: "UserRegistrations",
                column: "_ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrations__EmailConfirmationId",
                table: "UserRegistrations",
                column: "_EmailConfirmationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrations__PersonalDetailId",
                table: "UserRegistrations",
                column: "_PersonalDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrations__SmsConfirmationId",
                table: "UserRegistrations",
                column: "_SmsConfirmationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ContactId",
                table: "Users",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordId_PasswordCreatedOn",
                table: "Users",
                columns: new[] { "PasswordId", "PasswordCreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonalDetailId",
                table: "Users",
                column: "PersonalDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessTypes");

            migrationBuilder.DropTable(
                name: "IdentityProviders");

            migrationBuilder.DropTable(
                name: "MerchantCategoryCodes");

            migrationBuilder.DropTable(
                name: "MerchantRegistrations");

            migrationBuilder.DropTable(
                name: "MerchantRegistrationStatuses");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRegistrations");

            migrationBuilder.DropTable(
                name: "UserRegistrationStatuses");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "BusinessInfos");

            migrationBuilder.DropTable(
                name: "ConfirmationCodes");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Passwords");

            migrationBuilder.DropTable(
                name: "PersonalDetails");
        }
    }
}
