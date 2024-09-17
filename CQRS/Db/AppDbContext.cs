using Microsoft.EntityFrameworkCore;

namespace CQRS.Db
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(order => order.Id);

            // strongly typed ID conversion
            modelBuilder.Entity<Order>()
                .Property(order => order.Id)
                .HasConversion(id => id.Value, value => new(value));

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = OrderId.New(), Name = "Cup of coffee"},
                new Order { Id = OrderId.New(), Name = "Bottle of water"}
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("CqrsTest");
        }
    }
}
