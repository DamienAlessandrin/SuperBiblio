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

            Menu(bookRepository, authorRepository);

            Console.ReadLine();
        }

        private static void Menu(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            do
            {
                Functions functions = new Functions();

                functions.GetBooks(bookRepository);
                functions.GetAuthors(authorRepository);

                StringBuilder menu = new StringBuilder();
                menu.Append("*********************************************************************************************************\n");
                menu.Append("Liste des fonctionnalités :\n");
                menu.Append("- 1 : Créer un livre et l'attribuer à un auteur.\n");
                menu.Append("- 2 : Assigner un rayon à un livre. (Bientôt disponible)\n");

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
                        break;

                    case "3":
                        break;

                    case "q":
                        System.Environment.Exit(0);
                        break;
                }
            } while (true);
        }
    }
}