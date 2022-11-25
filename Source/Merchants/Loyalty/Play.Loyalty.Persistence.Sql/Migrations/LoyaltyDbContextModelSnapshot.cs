﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Play.Loyalty.Persistence.Sql.Persistence;

#nullable disable

namespace Play.Loyalty.Persistence.Sql.Migrations
{
    [DbContext(typeof(LoyaltyDbContext))]
    partial class LoyaltyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Play.Domain.Common.ValueObjects.MoneyValueObject", b =>
                {
                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("Amount");

                    b.Property<ushort>("NumericCurrencyCode")
                        .HasColumnType("int")
                        .HasColumnName("NumericCurrencyCode");

                    b.ToTable("MoneyValueObject");
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Aggregates.Member", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("_MerchantId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MerchantId");

                    b.Property<string>("_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("_Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Phone");

                    b.Property<string>("_RewardsId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("RewardsId");

                    b.Property<string>("_RewardsNumber")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("RewardsNumber");

                    b.HasKey("Id");

                    b.HasIndex("_RewardsId");

                    b.ToTable("Members", (string)null);
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Aggregates.Programs", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_DiscountProgramId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("DiscountProgramId");

                    b.Property<string>("_MerchantId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MerchantId");

                    b.Property<string>("_RewardProgramId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("RewardsProgramId");

                    b.HasKey("Id");

                    b.HasIndex("_DiscountProgramId");

                    b.HasIndex("_RewardProgramId");

                    b.ToTable("Programs", (string)null);
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Entities.Discount", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiscountProgramId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_ItemId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ItemId");

                    b.Property<decimal>("_Price.Amount")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("Amount");

                    b.Property<decimal>("_Price.NumericCurrencyCode")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("NumericCurrencyCode");

                    b.Property<string>("_VariationId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("VariationId");

                    b.HasKey("Id");

                    b.HasIndex("DiscountProgramId");

                    b.ToTable("Discounts", (string)null);
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Entities.RewardProgram", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("_IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("IsActive");

                    b.Property<long>("_PointsPerDollar")
                        .HasColumnType("bigint")
                        .HasColumnName("PointsPerDollar");

                    b.Property<long>("_PointsRequired")
                        .HasColumnType("bigint")
                        .HasColumnName("PointsRequired");

                    b.Property<decimal>("_RewardAmount.Amount")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("Amount");

                    b.Property<decimal>("_RewardAmount.NumericCurrencyCode")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("NumericCurrencyCode");

                    b.HasKey("Id");

                    b.ToTable("RewardProgram", (string)null);
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Entities.Rewards", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("_Balance.Amount")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("Amount");

                    b.Property<decimal>("_Balance.NumericCurrencyCode")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("NumericCurrencyCode");

                    b.Property<long>("_Points")
                        .HasColumnType("bigint")
                        .HasColumnName("Points");

                    b.HasKey("Id");

                    b.ToTable("Rewards", (string)null);
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Entitiesd.DiscountProgram", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_DiscountsId")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DiscountId");

                    b.Property<bool>("_IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("IsActive");

                    b.HasKey("Id");

                    b.ToTable("DiscountPrograms", (string)null);
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Aggregates.Member", b =>
                {
                    b.HasOne("Play.Loyalty.Domain.Entities.Rewards", "_Rewards")
                        .WithMany()
                        .HasForeignKey("_RewardsId");

                    b.Navigation("_Rewards");
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Aggregates.Programs", b =>
                {
                    b.HasOne("Play.Loyalty.Domain.Entitiesd.DiscountProgram", "_DiscountProgram")
                        .WithMany()
                        .HasForeignKey("_DiscountProgramId");

                    b.HasOne("Play.Loyalty.Domain.Entities.RewardProgram", "_RewardProgram")
                        .WithMany()
                        .HasForeignKey("_RewardProgramId");

                    b.Navigation("_DiscountProgram");

                    b.Navigation("_RewardProgram");
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Entities.Discount", b =>
                {
                    b.HasOne("Play.Loyalty.Domain.Entitiesd.DiscountProgram", null)
                        .WithMany("_Discounts")
                        .HasForeignKey("DiscountProgramId");
                });

            modelBuilder.Entity("Play.Loyalty.Domain.Entitiesd.DiscountProgram", b =>
                {
                    b.Navigation("_Discounts");
                });
#pragma warning restore 612, 618
        }
    }
}
