using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class TVShowConfiguration : IEntityTypeConfiguration<TVShow>
{
    public void Configure(EntityTypeBuilder<TVShow> builder)
    {
        builder.ToTable("TVShows");
        builder.HasMany(tv => tv.Seasons)
               .WithOne(s => s.TVShow)
               .HasForeignKey(s => s.TVShowId);
    }
}
