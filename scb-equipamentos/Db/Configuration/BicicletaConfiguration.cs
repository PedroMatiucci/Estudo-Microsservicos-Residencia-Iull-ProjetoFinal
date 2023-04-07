using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scb_equipamentos.Models;

namespace scb_equipamentos.Db.Configuration
{
    public class BicicletaConfiguration : IEntityTypeConfiguration<Bicicleta>
    {
        public void Configure(EntityTypeBuilder<Bicicleta> builder)
        {
            builder
            .Property(f => f.Status)
            .HasColumnName("Status");

            builder
            .Ignore(f => f.StatusEnum);

            builder.HasOne(b => b.Tranca)
            .WithOne(t => t.Bicicleta)
            .HasForeignKey<Tranca>(t => t.BicicletaId);
        }
    }
}
