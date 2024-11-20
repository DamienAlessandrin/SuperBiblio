using Microsoft.EntityFrameworkCore;
using SuperBiblio.Data.Models;

namespace SuperBiblio.Data
{
    public class DataContext : DbContext
    {
        public DbSet<BookModel> Book { get; set; }
        public DbSet<AuthorModel> Author { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated(); // Créer la BDD
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookModel>().HasData(
                new BookModel() { Id = 1, Title = "1984" }, // Auteur : George Orwell
                new BookModel() { Id = 2, Title = "L'Odyssée" }, // Auteur : Homère
                new BookModel() { Id = 3, Title = "Le Rouge et le Noir" } // Auteur : Stendhal
                );

            modelBuilder.Entity<AuthorModel>().HasData(
                new AuthorModel() { Id = 1, FirstName = "George", LastName = "Orwell" },
                new AuthorModel() { Id = 2, FirstName = "Homère" },
                new AuthorModel() { Id = 3, FirstName = "Stendhal" }
                );
        }
    }
}