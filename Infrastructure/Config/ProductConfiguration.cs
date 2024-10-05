using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // We define the precision and digits of the decimal explicitly.
        // This is needed to silence the warning:
        //
        // warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
        //   No store type was specified for the decimal property 'Price' on entity type
        //   'Product'. This will cause values to be silently truncated if they do not fit
        //   in the default precision and scale. Explicitly specify the SQL server column
        //   type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType',
        //   specify precision and scale using 'HasPrecision', or configure a value converter
        //   using 'HasConversion'.
        //
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
    }
}
