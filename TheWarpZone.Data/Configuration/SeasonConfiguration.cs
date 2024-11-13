using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasMany(s => s.Episodes)
               .WithOne(e => e.Season)
               .HasForeignKey(e => e.SeasonId);
    }
}
