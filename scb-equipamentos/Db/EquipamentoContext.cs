using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Db.Configuration;
using scb_equipamentos.Models;

namespace scb_equipamentos.Db
{
    public class EquipamentoContext : DbContext
    {
        public DbSet<Bicicleta> Bicicletas { get; set; }
        public DbSet<Tranca> Trancas { get; set; }
        public DbSet<Totem> Totems { get; set; }

        public EquipamentoContext(DbContextOptions<EquipamentoContext> opt) : base(opt)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BicicletaConfiguration());
            modelBuilder.ApplyConfiguration(new TrancaConfiguration());
        }


    }
}
