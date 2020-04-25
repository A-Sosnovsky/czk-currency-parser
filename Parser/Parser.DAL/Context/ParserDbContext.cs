using Microsoft.EntityFrameworkCore;

namespace Parser.DAL.Context
{
    internal class ParserDbContext : DbContext
    {
        public ParserDbContext()
        {
        }

        public ParserDbContext(DbContextOptions<ParserDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Parser;Trusted_Connection=True;;MultipleActiveResultSets=true;");
        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyValue> CurrencyValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyValue>()
                .HasOne(p => p.Currency)
                .WithMany(b => b.Values)
                .HasForeignKey(p => p.CurrencyId);
            
            modelBuilder.Entity<CurrencyValue>()
                .Property(p => p.UnitValue)
                .HasComputedColumnSql("[Value] / [Amount]");

            modelBuilder.Entity<CurrencyValue>()
                .HasIndex(b => new {b.CurrencyId, b.Date})
                .IsUnique();
            
            modelBuilder.Entity<Currency>()
                .HasIndex(b => new {b.Name})
                .IsUnique();
            
            modelBuilder.Entity<CurrencyValue>(entity =>
                entity.HasCheckConstraint("CK_CurrencyValue_Value", "[Value] >= 0"));

            base.OnModelCreating(modelBuilder);
        }
    }
}