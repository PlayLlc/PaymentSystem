using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Enums;

namespace Play.Underwriting.Persistence.Configuration;
/// <summary>
/// Specifications taken from : https://home.treasury.gov/system/files/126/dat_spec.txt
/// </summary>
internal class UnderwritingEntitiesConfiguration : IEntityTypeConfiguration<Individual>, IEntityTypeConfiguration<Address>, 
    IEntityTypeConfiguration<Alias>
{
    public void Configure(EntityTypeBuilder<Individual> builder)
    {
        builder.ToTable($"{nameof(Individual)}s");
        builder.HasKey(x => x.Number);
        builder.Property(x => x.Number).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(350);
        builder.Property(x => x.EntityType).HasConversion<string>(x => x, y => new EntityType(y)).HasMaxLength(12);
        builder.Property(x => x.Program).HasMaxLength(200);
        builder.Property(x => x.Title).HasMaxLength(200);
        builder.Property(x => x.VesselCallSign).HasMaxLength(8);
        builder.Property(x => x.VesselType).HasMaxLength(25);
        builder.Property(x => x.Tonnage).HasMaxLength(14);
        builder.Property(x => x.GrossRegisteredTonnage).HasMaxLength(8);
        builder.Property(x => x.VesselFlag).HasMaxLength(40);
        builder.Property(x => x.VesselOwner).HasMaxLength(150);
        builder.Property(x => x.Remarks).HasMaxLength(1000);

        builder.HasMany<Address>($"{nameof(Address)}es");
        builder.HasMany<Alias>($"{nameof(Alias)}es");

        builder.Navigation(x => x.Addresses).AutoInclude();
        builder.Navigation(x => x.Aliases).AutoInclude();
    }

    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable($"{nameof(Address)}es");
        builder.HasKey(x => x.Number);
        builder.Property(x => x.Number).ValueGeneratedNever();

        builder.Property(x => x.StreetAddress).HasMaxLength(750);
        builder.Property(x => x.Country).HasMaxLength(250);
        builder.Property(x => x.Remarks).HasMaxLength(200);
    }

    public void Configure(EntityTypeBuilder<Alias> builder)
    {
        builder.ToTable($"{nameof(Alias)}es");
        builder.HasKey(x => x.Number);
        builder.Property(x => x.Number).ValueGeneratedNever();

        var aliasNameBuilder = builder.OwnsOne(x => x.AliasName);

        aliasNameBuilder.Property(x => x.Type)
            .HasColumnName("Type");

        aliasNameBuilder.Property(x => x.Name)
            .HasColumnName("Name");

        builder.Property(x => x.Remarks).HasMaxLength(200);
    }
}
