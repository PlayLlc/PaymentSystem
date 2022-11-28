#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Play.Identity.Persistence.Sql.Migrations;

public partial class InitialMigration : Migration
{
    #region Instance Members

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("Addresses", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            ApartmentNumber = table.Column<string>("nvarchar(max)", nullable: true),
            City = table.Column<string>("nvarchar(max)", nullable: false),
            State = table.Column<string>("nvarchar(max)", nullable: false),
            StreetAddress = table.Column<string>("nvarchar(max)", nullable: false),
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
            Code = table.Column<long>("bigint", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
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

        migrationBuilder.CreateTable("IdentityProviders", table => new
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
            UserId = table.Column<string>("nvarchar(450)", nullable: false),
            CreatedOn = table.Column<DateTime>("datetime2", nullable: false),
            HashedPassword = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Passwords", x => new
            {
                x.UserId,
                x.CreatedOn
            });
        });

        migrationBuilder.CreateTable("PersonalDetails", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            DateOfBirth = table.Column<DateTime>("datetime2", nullable: false),
            LastFourOfSocial = table.Column<string>("nvarchar(max)", nullable: false)
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
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            AddressId = table.Column<string>("nvarchar(450)", nullable: true),
            BusinessInfoId = table.Column<string>("nvarchar(450)", nullable: true),
            CompanyName = table.Column<string>("nvarchar(max)", nullable: false),
            RegistrationDate = table.Column<DateTime>("datetime2", nullable: false),
            Status = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_MerchantRegistrations", x => x.Id);
            table.ForeignKey("FK_MerchantRegistrations_Addresses_AddressId", x => x.AddressId, "Addresses", "Id");
            table.ForeignKey("FK_MerchantRegistrations_BusinessInfos_BusinessInfoId", x => x.BusinessInfoId, "BusinessInfos", "Id");
        });

        migrationBuilder.CreateTable("Merchants", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            AddressId = table.Column<string>("nvarchar(450)", nullable: true),
            BusinessInfoId = table.Column<string>("nvarchar(450)", nullable: true),
            CompanyName = table.Column<string>("nvarchar(max)", nullable: false),
            IsActive = table.Column<bool>("bit", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Merchants", x => x.Id);
            table.ForeignKey("FK_Merchants_Addresses_AddressId", x => x.AddressId, "Addresses", "Id");
            table.ForeignKey("FK_Merchants_BusinessInfos_BusinessInfoId", x => x.BusinessInfoId, "BusinessInfos", "Id");
        });

        migrationBuilder.CreateTable("UserRegistrations", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            AddressId = table.Column<string>("nvarchar(450)", nullable: true),
            ContactId = table.Column<string>("nvarchar(450)", nullable: true),
            EmailConfirmationId = table.Column<string>("nvarchar(450)", nullable: true),
            HasEmailBeenVerified = table.Column<bool>("bit", nullable: false),
            HasPhoneBeenVerified = table.Column<bool>("bit", nullable: false),
            HashedPassword = table.Column<string>("nvarchar(max)", nullable: false),
            PersonalDetailId = table.Column<string>("nvarchar(450)", nullable: true),
            RegistrationDate = table.Column<DateTime>("datetime2", nullable: false),
            SmsConfirmationId = table.Column<string>("nvarchar(450)", nullable: true),
            Status = table.Column<string>("nvarchar(max)", nullable: false),
            Username = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_UserRegistrations", x => x.Id);
            table.ForeignKey("FK_UserRegistrations_Addresses_AddressId", x => x.AddressId, "Addresses", "Id");
            table.ForeignKey("FK_UserRegistrations_ConfirmationCodes_EmailConfirmationId", x => x.EmailConfirmationId, "ConfirmationCodes", "Id");
            table.ForeignKey("FK_UserRegistrations_ConfirmationCodes_SmsConfirmationId", x => x.SmsConfirmationId, "ConfirmationCodes", "Id");
            table.ForeignKey("FK_UserRegistrations_Contacts_ContactId", x => x.ContactId, "Contacts", "Id");
            table.ForeignKey("FK_UserRegistrations_PersonalDetails_PersonalDetailId", x => x.PersonalDetailId, "PersonalDetails", "Id");
        });

        migrationBuilder.CreateTable("Users", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            ContactId = table.Column<string>("nvarchar(450)", nullable: false),
            IsActive = table.Column<bool>("bit", nullable: false),
            AddressId = table.Column<string>("nvarchar(450)", nullable: true),
            MerchantId = table.Column<string>("nvarchar(max)", nullable: false),
            PasswordCreatedOn = table.Column<DateTime>("datetime2", nullable: true),
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
            table.ForeignKey("FK_Users_Passwords_PasswordId_PasswordCreatedOn", x => new
            {
                x.PasswordId,
                x.PasswordCreatedOn
            }, "Passwords", new[] {"UserId", "CreatedOn"});
            table.ForeignKey("FK_Users_PersonalDetails_PersonalDetailId", x => x.PersonalDetailId, "PersonalDetails", "Id");
        });

        migrationBuilder.InsertData("BusinessTypes", "Value", new object[] {"Exempt", "LimitedLiability", "NonProfit", "Partnership", "SoleProprietorship"});

        migrationBuilder.InsertData("MerchantCategoryCodes", new[] {"Value", "Name"},
            new object[,] {{4214, "Delivery"}, {7392, "Consulting"}, {8111, "LegalServices"}, {8351, "Childcare"}, {8931, "Accounting"}});

        migrationBuilder.InsertData("MerchantRegistrationStatuses", "Value", new object[] {"Approved", "Expired", "Rejected", "WaitingForRiskAnalysis"});

        migrationBuilder.InsertData("Roles", new[] {"Id", "ConcurrencyStamp", "Name", "NormalizedName"},
            new object[,]
            {
                {"Administrator", "f69f382c-21aa-4719-9183-239d9d1ef2c1", "Administrator", "ADMINISTRATOR"},
                {"SalesAssociate", "b79ea52c-710c-41ab-8b93-418111d18fec", "SalesAssociate", "SALESASSOCIATE"},
                {"SuperAdmin", "52ae1e70-cf4d-4f4d-b244-8d9d506fd871", "SuperAdmin", "SUPERADMIN"}
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

        migrationBuilder.CreateIndex("IX_MerchantRegistrations_AddressId", "MerchantRegistrations", "AddressId");

        migrationBuilder.CreateIndex("IX_MerchantRegistrations_BusinessInfoId", "MerchantRegistrations", "BusinessInfoId");

        migrationBuilder.CreateIndex("IX_Merchants_AddressId", "Merchants", "AddressId");

        migrationBuilder.CreateIndex("IX_Merchants_BusinessInfoId", "Merchants", "BusinessInfoId");

        migrationBuilder.CreateIndex("IX_UserRegistrations_AddressId", "UserRegistrations", "AddressId");

        migrationBuilder.CreateIndex("IX_UserRegistrations_ContactId", "UserRegistrations", "ContactId");

        migrationBuilder.CreateIndex("IX_UserRegistrations_EmailConfirmationId", "UserRegistrations", "EmailConfirmationId");

        migrationBuilder.CreateIndex("IX_UserRegistrations_PersonalDetailId", "UserRegistrations", "PersonalDetailId");

        migrationBuilder.CreateIndex("IX_UserRegistrations_SmsConfirmationId", "UserRegistrations", "SmsConfirmationId");

        migrationBuilder.CreateIndex("IX_Users_AddressId", "Users", "AddressId");

        migrationBuilder.CreateIndex("IX_Users_ContactId", "Users", "ContactId");

        migrationBuilder.CreateIndex("IX_Users_PasswordId_PasswordCreatedOn", "Users", new[] {"PasswordId", "PasswordCreatedOn"});

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