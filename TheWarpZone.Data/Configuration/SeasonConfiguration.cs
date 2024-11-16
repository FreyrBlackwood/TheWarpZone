using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheWarpZone.Common.Constraints;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable("Seasons");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SeasonNumber)
               .IsRequired();

        builder.HasMany(s => s.Episodes)
               .WithOne(e => e.Season)
               .HasForeignKey(e => e.SeasonId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.TVShow)
               .WithMany(tv => tv.Seasons)
               .HasForeignKey(s => s.TVShowId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
