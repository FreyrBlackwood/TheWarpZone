using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheWarpZone.Common.Constraints;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
               .IsRequired()
               .HasMaxLength(MovieConstraints.TitleMaxLength);

        builder.Property(m => m.Description)
               .HasMaxLength(MovieConstraints.DescriptionMaxLength);

        builder.Property(m => m.ReleaseDate)
               .IsRequired();

        builder.Property(m => m.Director)
               .IsRequired()
               .HasMaxLength(MovieConstraints.DirectorMaxLength);

        builder.HasMany(m => m.Tags)
               .WithMany(t => t.Movies)
               .UsingEntity(j => j.ToTable("MovieTags"));

        builder.HasMany(m => m.Reviews)
               .WithOne(r => r.Movie)
               .HasForeignKey(r => r.MovieId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Ratings)
               .WithOne(r => r.Movie)
               .HasForeignKey(r => r.MovieId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.CastMembers)
               .WithOne(c => c.Movie)
               .HasForeignKey(c => c.MovieId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
