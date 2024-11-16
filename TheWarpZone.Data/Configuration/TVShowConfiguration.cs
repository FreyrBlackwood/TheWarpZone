using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheWarpZone.Common.Constraints;

public class TVShowConfiguration : IEntityTypeConfiguration<TVShow>
{
    public void Configure(EntityTypeBuilder<TVShow> builder)
    {
        builder.ToTable("TVShows");

        builder.HasKey(tv => tv.Id);

        builder.Property(tv => tv.Title)
               .IsRequired()
               .HasMaxLength(TVShowConstraints.TitleMaxLength);

        builder.Property(tv => tv.Description)
               .HasMaxLength(TVShowConstraints.DescriptionMaxLength);

        builder.Property(tv => tv.ReleaseDate)
               .IsRequired();

        builder.HasMany(tv => tv.Tags)
               .WithMany(t => t.TVShows)
               .UsingEntity(j => j.ToTable("TVShowTags"));

        builder.HasMany(tv => tv.Seasons)
               .WithOne(s => s.TVShow)
               .HasForeignKey(s => s.TVShowId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(tv => tv.Reviews)
               .WithOne(r => r.TVShow)
               .HasForeignKey(r => r.TVShowId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(tv => tv.Ratings)
               .WithOne(r => r.TVShow)
               .HasForeignKey(r => r.TVShowId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(tv => tv.CastMembers)
               .WithOne(c => c.TVShow)
               .HasForeignKey(c => c.TVShowId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
