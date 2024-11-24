using Microsoft.EntityFrameworkCore;
using SuperBiblio.Data.Models;

namespace SuperBiblio.Data
{
    public class DataContext : DbContext
    {
        public DbSet<BookModel> Book { get; set; }
        public DbSet<AuthorModel> Author { get; set; }

        public DbSet<ShelfModel> Shelf { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            //Database.EnsureDeleted(); // Supprime les données pour les renouveler en cas de changements //TODO: (A ENLEVER EN PROD!!!!)
            Database.EnsureCreated(); // Créer la BDD
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookModel>().HasData(
                new BookModel() { Id = 1, Title = "1984", AuthorModelId = 1, ShelfModelId = 1 },
                new BookModel() { Id = 2, Title = "L'Odyssée", AuthorModelId = 2 },
                new BookModel() { Id = 3, Title = "Le Rouge et le Noir", AuthorModelId = 3 }
                );

            modelBuilder.Entity<AuthorModel>().HasData(
                new AuthorModel() { Id = 1, FirstName = "George", LastName = "Orwell" },
                new AuthorModel() { Id = 2, FirstName = "Homère" },
                new AuthorModel() { Id = 3, FirstName = "Stendhal" }
                );

            modelBuilder.Entity<ShelfModel>().HasData(
                new ShelfModel() { Id = 1, Name = "Horreur" },
                new ShelfModel() { Id = 2, Name = "Humour" },
                new ShelfModel() { Id = 3, Name = "Policier" }
                );
        }
    }
}