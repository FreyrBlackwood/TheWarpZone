using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheWarpZone.Common.Constraints;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.ToTable("Episodes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EpisodeNumber)
               .IsRequired();

        builder.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(EpisodeConstraints.EpisodeTitleMaxLength);

        builder.Property(e => e.EpisodeDescription)
               .HasMaxLength(EpisodeConstraints.EpisodeDescriptionMaxLength);

        builder.HasOne(e => e.Season)
               .WithMany(s => s.Episodes)
               .HasForeignKey(e => e.SeasonId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
