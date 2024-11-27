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
            IMemberRepository memberRepository = new ApiMemberRepository(URL);

            Menu(bookRepository, authorRepository, shelfRepository, memberRepository);

            Console.ReadLine();
        }

        private static void Menu(IBookRepository bookRepository, IAuthorRepository authorRepository, IShelfRepository shelfRepository, IMemberRepository memberRepository)
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
                menu.Append("- 5 : Rechercher un livre par titre.\n");
                menu.Append("- 6 : Emprunter un livre.\n");
                menu.Append("- 7 : Rendre un livre.\n");

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

                    case "5":
                        functions.GetBooksByTitle(bookRepository);
                        break;

                    case "6":
                        functions.BorrowBook(memberRepository, bookRepository);
                        break;

                    case "7":
                        functions.ReturnBorrowBook(memberRepository, bookRepository);
                        break;

                    case "q":
                        System.Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Choix indisponible.");
                        break;
                }

                Console.WriteLine("Appuyer sur une touche pour continuer ...");
                Console.ReadLine();
            } while (true);
        }
    }
}