using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheWarpZone.Common.Constraints;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Comment)
               .IsRequired()
               .HasMaxLength(ReviewConstraints.CommentMaxLength);

        builder.Property(r => r.PostedDate)
               .IsRequired();

        builder.HasOne(r => r.Movie)
               .WithMany(m => m.Reviews)
               .HasForeignKey(r => r.MovieId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.TVShow)
               .WithMany(tv => tv.Reviews)
               .HasForeignKey(r => r.TVShowId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
