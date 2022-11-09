﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Play.Merchants.Underwriting.Persistence.Persistence;

#nullable disable

namespace Play.Underwriting.Persistence.Sql.Migrations
{
    [DbContext(typeof(UnderwritingDbContext))]
    partial class UnderwritingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Play.Underwriting.Domain.Entities.Address", b =>
                {
                    b.Property<decimal>("Number")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<decimal>("IndividualNumber")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(750)
                        .HasColumnType("nvarchar(750)");

                    b.HasKey("Number");

                    b.HasIndex("IndividualNumber");

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("Play.Underwriting.Domain.Entities.AlternateIdentity", b =>
                {
                    b.Property<decimal>("Number")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("IndividualNumber")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("Number");

                    b.HasIndex("IndividualNumber");

                    b.ToTable("AlternateIdentities", (string)null);
                });

            modelBuilder.Entity("Play.Underwriting.Domain.Entities.Individual", b =>
                {
                    b.Property<decimal>("Number")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("GrossRegisteredTonnage")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("Program")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Tonnage")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("VesselCallSign")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("VesselFlag")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("VesselOwner")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("VesselType")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Number");

                    b.ToTable("Individuals", (string)null);
                });

            modelBuilder.Entity("Play.Underwriting.Domain.Entities.Address", b =>
                {
                    b.HasOne("Play.Underwriting.Domain.Entities.Individual", null)
                        .WithMany("Addresses")
                        .HasForeignKey("IndividualNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Play.Underwriting.Domain.Entities.AlternateIdentity", b =>
                {
                    b.HasOne("Play.Underwriting.Domain.Entities.Individual", null)
                        .WithMany("AlternateIdentities")
                        .HasForeignKey("IndividualNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Play.Underwriting.Domain.Entities.Individual", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("AlternateIdentities");
                });
#pragma warning restore 612, 618
        }
    }
}
