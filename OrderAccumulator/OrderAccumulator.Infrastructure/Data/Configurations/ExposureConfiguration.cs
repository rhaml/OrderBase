using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAccumulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccumulator.Infrastructure.Data.Configurations
{
    public class ExposureConfiguration : IEntityTypeConfiguration<Exposure>
    {
        public void Configure(EntityTypeBuilder<Exposure> builder)
        {
            builder.ToTable("Exposures");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Symbol).IsRequired();
            builder.Property(e => e.Value).HasPrecision(18, 2);
            builder.HasIndex(e => e.Symbol).IsUnique();
        }
    }
}
