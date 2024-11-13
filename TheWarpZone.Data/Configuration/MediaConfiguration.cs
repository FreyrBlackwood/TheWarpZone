using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasMany(m => m.Tags)
               .WithMany(t => t.Media)
               .UsingEntity(j => j.ToTable("MediaTags"));

        builder.HasMany(m => m.CastMembers)
               .WithOne(c => c.Media)
               .HasForeignKey(c => c.MediaId);
    }
}
