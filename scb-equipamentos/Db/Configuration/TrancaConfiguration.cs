using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scb_equipamentos.Models;

namespace scb_equipamentos.Db.Configuration
{
    public class TrancaConfiguration : IEntityTypeConfiguration<Tranca>
    {
        public void Configure(EntityTypeBuilder<Tranca> builder)
        {
            builder
            .Property(f => f.Status)
            .HasColumnName("Status");

            builder
            .Ignore(f => f.StatusEnum);

        }
    }
}
