﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Play.TimeClock.Persistence.Sql.Persistence;

#nullable disable

namespace Play.TimeClock.Persistence.Sql.Migrations
{
    [DbContext(typeof(TimeClockDbContext))]
    partial class TimeClockDbContextModelSnapshot : ModelSnapshot
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

            modelBuilder.Entity("Play.TimeClock.Domain.Aggregates.Employee", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("_MerchantId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MerchantId");

                    b.Property<string>("_TimeEntriesId")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TimeEntryId");

                    b.Property<string>("_TimePuncherId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("TimePuncherId");

                    b.Property<string>("_UserId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("_TimePuncherId");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("Play.TimeClock.Domain.Entities.TimeEntry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("EmployeeId1");

                    b.Property<string>("_EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("EmployeeId");

                    b.Property<DateTime>("_EndTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("EndTime");

                    b.Property<DateTime>("_StartTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("TimeEntries", (string)null);
                });

            modelBuilder.Entity("Play.TimeClock.Domain.Entities.TimePuncher", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("_ClockedInAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ClockedInAt");

                    b.Property<string>("_EmployeeId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("EmployeeId");

                    b.Property<string>("_TimeClockStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TimeClockStatus");

                    b.HasKey("Id");

                    b.ToTable("TimeClocks", (string)null);
                });

            modelBuilder.Entity("Play.TimeClock.Domain.ValueObject.TimeClockStatus", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Value");

                    b.ToTable("TimeClockStatus", (string)null);

                    b.HasData(
                        new
                        {
                            Value = "ClockedIn"
                        },
                        new
                        {
                            Value = "ClockedOut"
                        });
                });

            modelBuilder.Entity("Play.TimeClock.Domain.Aggregates.Employee", b =>
                {
                    b.HasOne("Play.TimeClock.Domain.Entities.TimePuncher", "_TimePuncher")
                        .WithMany()
                        .HasForeignKey("_TimePuncherId");

                    b.Navigation("_TimePuncher");
                });

            modelBuilder.Entity("Play.TimeClock.Domain.Entities.TimeEntry", b =>
                {
                    b.HasOne("Play.TimeClock.Domain.Aggregates.Employee", null)
                        .WithMany("_TimeEntries")
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("Play.TimeClock.Domain.Aggregates.Employee", b =>
                {
                    b.Navigation("_TimeEntries");
                });
#pragma warning restore 612, 618
        }
    }
}