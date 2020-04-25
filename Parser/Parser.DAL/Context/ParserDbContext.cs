using System;
using Microsoft.EntityFrameworkCore;

namespace Parser.DAL.Context
{
    internal class ParserDbContext : DbContext
    {
        private static string ConnectionString { get; set; }

        public ParserDbContext()
        {
        }

        public ParserDbContext(DbContextOptions<ParserDbContext> options) : base(options)
        {
        }

        public static void SetConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
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

            modelBuilder.ConfigureDbFunctions();

            base.OnModelCreating(modelBuilder);
        }
    }
}