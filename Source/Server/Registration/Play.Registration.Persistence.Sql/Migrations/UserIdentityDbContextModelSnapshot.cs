﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Registration.Persistence.Sql.Persistence;

#nullable disable

namespace Play.Identity.Persistence.Sql.Migrations
{
    [DbContext(typeof(RegistrationDbContext))]
    partial class UserIdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("Play.Domain.Common.Entities.Address", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApartmentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("Play.Domain.Common.Entities.Contact", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contacts", (string)null);
                });

            modelBuilder.Entity("Play.Domain.Common.ValueObjects.State", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Value");

                    b.ToTable("States", (string)null);

                    b.HasData(
                        new
                        {
                            Value = "Alabama"
                        },
                        new
                        {
                            Value = "Alaska"
                        },
                        new
                        {
                            Value = "Arizona"
                        },
                        new
                        {
                            Value = "Arkansas"
                        },
                        new
                        {
                            Value = "California"
                        },
                        new
                        {
                            Value = "Colorado"
                        },
                        new
                        {
                            Value = "Connecticut"
                        },
                        new
                        {
                            Value = "Delaware"
                        },
                        new
                        {
                            Value = "DistrictOfColumbia"
                        },
                        new
                        {
                            Value = "Florida"
                        },
                        new
                        {
                            Value = "Georgia"
                        },
                        new
                        {
                            Value = "Hawaii"
                        },
                        new
                        {
                            Value = "Idaho"
                        },
                        new
                        {
                            Value = "Illinois"
                        },
                        new
                        {
                            Value = "Indiana"
                        },
                        new
                        {
                            Value = "Iowa"
                        },
                        new
                        {
                            Value = "Kansas"
                        },
                        new
                        {
                            Value = "Kentucky"
                        },
                        new
                        {
                            Value = "Louisiana"
                        },
                        new
                        {
                            Value = "Maine"
                        },
                        new
                        {
                            Value = "Maryland"
                        },
                        new
                        {
                            Value = "Massachusetts"
                        },
                        new
                        {
                            Value = "Michigan"
                        },
                        new
                        {
                            Value = "Minnesota"
                        },
                        new
                        {
                            Value = "Mississippi"
                        },
                        new
                        {
                            Value = "Missouri"
                        },
                        new
                        {
                            Value = "Montana"
                        },
                        new
                        {
                            Value = "Nebraska"
                        },
                        new
                        {
                            Value = "Nevada"
                        },
                        new
                        {
                            Value = "NewHampshire"
                        },
                        new
                        {
                            Value = "NewJersey"
                        },
                        new
                        {
                            Value = "NewMexico"
                        },
                        new
                        {
                            Value = "NewYork"
                        },
                        new
                        {
                            Value = "NorthCarolina"
                        },
                        new
                        {
                            Value = "NorthDakota"
                        },
                        new
                        {
                            Value = "Ohio"
                        },
                        new
                        {
                            Value = "Oklahoma"
                        },
                        new
                        {
                            Value = "Oregon"
                        },
                        new
                        {
                            Value = "Pennsylvania"
                        },
                        new
                        {
                            Value = "RhodeIsland"
                        },
                        new
                        {
                            Value = "SouthCarolina"
                        },
                        new
                        {
                            Value = "SouthDakota"
                        },
                        new
                        {
                            Value = "Tennessee"
                        },
                        new
                        {
                            Value = "Texas"
                        },
                        new
                        {
                            Value = "Utah"
                        },
                        new
                        {
                            Value = "Vermont"
                        },
                        new
                        {
                            Value = "Virginia"
                        },
                        new
                        {
                            Value = "Washington"
                        },
                        new
                        {
                            Value = "WestVirginia"
                        },
                        new
                        {
                            Value = "Wisconsin"
                        },
                        new
                        {
                            Value = "Wyoming"
                        });
                });

            modelBuilder.Entity("Play.Identity.Domain.Aggregates.Merchant", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_AddressId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("AddressId");

                    b.Property<string>("_BusinessInfoId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("BusinessInfoId");

                    b.Property<string>("_CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CompanyName");

                    b.Property<bool>("_IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("IsActive");

                    b.HasKey("Id");

                    b.HasIndex("_AddressId");

                    b.HasIndex("_BusinessInfoId");

                    b.ToTable("Merchants", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Aggregates.MerchantRegistration", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_AddressId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("AddressId");

                    b.Property<string>("_BusinessInfoId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("BusinessInfoId");

                    b.Property<string>("_CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CompanyName");

                    b.Property<DateTime>("_RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("RegistrationDate");

                    b.Property<string>("_Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.HasIndex("_AddressId");

                    b.HasIndex("_BusinessInfoId");

                    b.ToTable("MerchantRegistrations", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Aggregates.UserRegistration", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_AddressId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("AddressId");

                    b.Property<string>("_ContactId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ContactId");

                    b.Property<string>("_EmailConfirmationId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("EmailConfirmationId");

                    b.Property<bool>("_HasEmailBeenVerified")
                        .HasColumnType("bit")
                        .HasColumnName("HasEmailBeenVerified");

                    b.Property<bool>("_HasPhoneBeenVerified")
                        .HasColumnType("bit")
                        .HasColumnName("HasPhoneBeenVerified");

                    b.Property<string>("_HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("HashedPassword");

                    b.Property<string>("_MerchantId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MerchantId");

                    b.Property<string>("_PersonalDetailId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("PersonalDetailId");

                    b.Property<DateTime>("_RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("RegistrationDate");

                    b.Property<string>("_SmsConfirmationId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("SmsConfirmationId");

                    b.Property<string>("_Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Status");

                    b.Property<string>("_Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Username");

                    b.HasKey("Id");

                    b.HasIndex("_AddressId");

                    b.HasIndex("_ContactId");

                    b.HasIndex("_EmailConfirmationId");

                    b.HasIndex("_PersonalDetailId");

                    b.HasIndex("_SmsConfirmationId");

                    b.ToTable("UserRegistrations", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Entities.BusinessInfo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BusinessType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<ushort>("MerchantCategoryCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BusinessInfos", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Entities.EmailConfirmationCode", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Code")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("EmailConfirmationCodes", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Entities.Password", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserId");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id", "CreatedOn");

                    b.ToTable("Passwords", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Entities.PersonalDetail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastFourOfSocial")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PersonalDetails", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Entities.SmsConfirmationCode", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Code")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SmsConfirmationCodes", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.ValueObjects.BusinessType", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Value");

                    b.ToTable("BusinessTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Value = "Exempt"
                        },
                        new
                        {
                            Value = "LimitedLiability"
                        },
                        new
                        {
                            Value = "NonProfit"
                        },
                        new
                        {
                            Value = "Partnership"
                        },
                        new
                        {
                            Value = "SoleProprietorship"
                        });
                });

            modelBuilder.Entity("Play.Identity.Domain.ValueObjects.MerchantCategoryCode", b =>
                {
                    b.Property<int>("Value")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Value"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Value");

                    b.ToTable("MerchantCategoryCodes", (string)null);

                    b.HasData(
                        new
                        {
                            Value = 4214,
                            Name = "Delivery"
                        },
                        new
                        {
                            Value = 7392,
                            Name = "Consulting"
                        },
                        new
                        {
                            Value = 8111,
                            Name = "LegalServices"
                        },
                        new
                        {
                            Value = 8351,
                            Name = "Childcare"
                        },
                        new
                        {
                            Value = 8931,
                            Name = "Accounting"
                        });
                });

            modelBuilder.Entity("Play.Identity.Domain.ValueObjects.MerchantRegistrationStatus", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Value");

                    b.ToTable("MerchantRegistrationStatuses", (string)null);

                    b.HasData(
                        new
                        {
                            Value = "Approved"
                        },
                        new
                        {
                            Value = "Expired"
                        },
                        new
                        {
                            Value = "Rejected"
                        },
                        new
                        {
                            Value = "WaitingForRiskAnalysis"
                        });
                });

            modelBuilder.Entity("Play.Identity.Domain.ValueObjects.UserRegistrationStatus", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Value");

                    b.ToTable("UserRegistrationStatuses", (string)null);

                    b.HasData(
                        new
                        {
                            Value = "Approved"
                        },
                        new
                        {
                            Value = "Expired"
                        },
                        new
                        {
                            Value = "Rejected"
                        },
                        new
                        {
                            Value = "WaitingForEmailVerification"
                        },
                        new
                        {
                            Value = "WaitingForRiskAnalysis"
                        },
                        new
                        {
                            Value = "WaitingForSmsVerification"
                        });
                });

            modelBuilder.Entity("Play.Identity.Persistence.Sql.Entities.IdentityProviders", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Properties")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Scheme")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("IdentityProviders");
                });

            modelBuilder.Entity("Play.Identity.Persistence.Sql.Entities.RoleIdentity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "Administrator",
                            ConcurrencyStamp = "d20e0d0b-2309-46fc-a024-cbbeb8a07b8e",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "SalesAssociate",
                            ConcurrencyStamp = "00fee480-efaf-42c2-8526-0abaebebb527",
                            Name = "SalesAssociate",
                            NormalizedName = "SALESASSOCIATE"
                        },
                        new
                        {
                            Id = "SuperAdmin",
                            ConcurrencyStamp = "32ecde1f-b877-404d-847b-ae24c613f6a4",
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        });
                });

            modelBuilder.Entity("Play.Identity.Persistence.Sql.Entities.UserIdentity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AddressId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("IsActive");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MerchantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MerchantId");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PasswordCreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PersonalDetailId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TerminalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TerminalId");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("ContactId");

                    b.HasIndex("PersonalDetailId");

                    b.HasIndex("PasswordId", "PasswordCreatedOn");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Play.Identity.Domain.Aggregates.Merchant", b =>
                {
                    b.HasOne("Play.Domain.Common.Entities.Address", "_Address")
                        .WithMany()
                        .HasForeignKey("_AddressId");

                    b.HasOne("Play.Identity.Domain.Entities.BusinessInfo", "_BusinessInfo")
                        .WithMany()
                        .HasForeignKey("_BusinessInfoId");

                    b.Navigation("_Address");

                    b.Navigation("_BusinessInfo");
                });

            modelBuilder.Entity("Play.Identity.Domain.Aggregates.MerchantRegistration", b =>
                {
                    b.HasOne("Play.Domain.Common.Entities.Address", "_Address")
                        .WithMany()
                        .HasForeignKey("_AddressId");

                    b.HasOne("Play.Identity.Domain.Entities.BusinessInfo", "_BusinessInfo")
                        .WithMany()
                        .HasForeignKey("_BusinessInfoId");

                    b.Navigation("_Address");

                    b.Navigation("_BusinessInfo");
                });

            modelBuilder.Entity("Play.Identity.Domain.Aggregates.UserRegistration", b =>
                {
                    b.HasOne("Play.Domain.Common.Entities.Address", "_Address")
                        .WithMany()
                        .HasForeignKey("_AddressId");

                    b.HasOne("Play.Domain.Common.Entities.Contact", "_Contact")
                        .WithMany()
                        .HasForeignKey("_ContactId");

                    b.HasOne("Play.Identity.Domain.Entities.EmailConfirmationCode", "_EmailConfirmation")
                        .WithMany()
                        .HasForeignKey("_EmailConfirmationId");

                    b.HasOne("Play.Identity.Domain.Entities.PersonalDetail", "_PersonalDetail")
                        .WithMany()
                        .HasForeignKey("_PersonalDetailId");

                    b.HasOne("Play.Identity.Domain.Entities.SmsConfirmationCode", "_SmsConfirmation")
                        .WithMany()
                        .HasForeignKey("_SmsConfirmationId");

                    b.Navigation("_Address");

                    b.Navigation("_Contact");

                    b.Navigation("_EmailConfirmation");

                    b.Navigation("_PersonalDetail");

                    b.Navigation("_SmsConfirmation");
                });

            modelBuilder.Entity("Play.Identity.Persistence.Sql.Entities.UserIdentity", b =>
                {
                    b.HasOne("Play.Domain.Common.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("Play.Domain.Common.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Play.Identity.Domain.Entities.PersonalDetail", "PersonalDetail")
                        .WithMany()
                        .HasForeignKey("PersonalDetailId");

                    b.HasOne("Play.Identity.Domain.Entities.Password", "Password")
                        .WithMany()
                        .HasForeignKey("PasswordId", "PasswordCreatedOn");

                    b.Navigation("Address");

                    b.Navigation("Contact");

                    b.Navigation("Password");

                    b.Navigation("PersonalDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
