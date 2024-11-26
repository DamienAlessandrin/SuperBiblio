using EfCore.Data.Repositories;
using SuperBiblio.Data.Repositories;
using SuperBiblio.Data.Repositories.Api;
using System.Text;

namespace SuperBiblio.Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string URL = "http://localhost:5259/api/";

            Thread.Sleep(5000); // Attente du serveur

            IBookRepository bookRepository = new ApiBookRepository(URL);
            IAuthorRepository authorRepository = new ApiAuthorRepository(URL);
            IShelfRepository shelfRepository = new ApiShelfRepository(URL);

            Menu(bookRepository, authorRepository, shelfRepository);

            Console.ReadLine();
        }

        private static void Menu(IBookRepository bookRepository, IAuthorRepository authorRepository, IShelfRepository shelfRepository)
        {
            do
            {
                Functions functions = new Functions();

                StringBuilder menu = new StringBuilder();
                menu.Append("*********************************************************************************************************\n");
                menu.Append("Liste des fonctionnalités :\n");
                menu.Append("- 1 : Créer un livre et l'attribuer à un auteur.\n");
                menu.Append("- 2 : Assigner un rayon à un livre.\n");
                menu.Append("- 3 : Lister les livres par auteur.\n");
                menu.Append("- 4 : Lister les livres par rayon.\n");

                menu.Append("\n\nq : Quitter\n");
                menu.Append("*********************************************************************************************************\n");
                menu.Append("Option choisie : ");
                Console.Write(menu.ToString());

                string reponse = Console.ReadLine();
                switch (reponse)
                {
                    case "1":
                        functions.CreateBook(bookRepository, authorRepository);
                        break;

                    case "2":
                        functions.AssignShelf(shelfRepository, bookRepository);
                        break;

                    case "3":
                        functions.GetAuthors(authorRepository);
                        functions.GetBooksByAuthor(bookRepository, authorRepository);
                        break;

                    case "4":
                        functions.GetBooksByShelf(bookRepository, shelfRepository);
                        break;

                    case "q":
                        System.Environment.Exit(0);
                        break;
                }
            } while (true);
        }
    }
}