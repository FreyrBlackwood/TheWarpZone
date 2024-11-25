using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserMediaListConfiguration : IEntityTypeConfiguration<UserMediaList>
{
    public void Configure(EntityTypeBuilder<UserMediaList> builder)
    {
        builder.ToTable("UserMediaLists");

        builder.HasKey(uml => uml.Id);

        builder.Property(uml => uml.Status)
               .IsRequired();

        builder.Property(uml => uml.UserId)
               .IsRequired();

        builder.HasOne(uml => uml.Movie)
               .WithMany()
               .HasForeignKey(uml => uml.MovieId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uml => uml.TVShow)
               .WithMany()
               .HasForeignKey(uml => uml.TVShowId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uml => uml.User)
               .WithMany()
               .HasForeignKey(uml => uml.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
