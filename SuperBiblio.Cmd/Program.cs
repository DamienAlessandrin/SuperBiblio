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

            Thread.Sleep(2000); // Attente du serveur

            IBookRepository bookRepository = new ApiBookRepository(URL);
            IAuthorRepository authorRepository = new ApiAuthorRepository(URL);

            GetBooks(bookRepository);

            GetAuthors(authorRepository);

            Console.ReadLine();
        }

        //TODO: Fonction pour ajouter un livre à un auteur (créer un livre en choisissant son auteur)
        //TODO: Fonction pour créer un auteur ou ajouter un choix dans la liste des auterus pour créer l'auteur s'il n'existe pas
        // FuncCreateBook --> Choose title
        //                --> Select Author --> FuncSelectAuthor --> Select author existing
        //                                                       --> Select new author (last option) --> FuncCreateAuthor --> Choose LastName
        //                                                                                                                --> Choose FirstName (optionnal)


        private static void GetBooks(IBookRepository repository)
        {
            var books = repository.Get().Result;
            if (books == null)
            {
                Console.WriteLine("Erreur Api");
                return;
            }

            StringBuilder message = new StringBuilder();
            foreach (var book in books)
                message.Append($"\"{book.Title}\" écrit par {book.Author.LastName} {book.Author.FirstName} (Id :{book.Id})\n");

            Console.WriteLine(message.ToString());
        }

        private static void GetAuthors(IAuthorRepository repository)
        {
            var authors = repository.Get().Result;
            if (authors == null)
            {
                Console.WriteLine("Erreur Api");
                return;
            }

            StringBuilder message = new StringBuilder();
            foreach (var author in authors)
                message.Append($"{author.LastName} {author.FirstName} (Id :{author.Id})\n");

            Console.WriteLine(message.ToString());
        }

        //private static void GetPerson(IPersonRepository repository)
        //{
        //    while (true)
        //    {
        //        int id = GetId("id de la personne : ");
        //        if (id == 0)
        //            return;

        //        var model = repository.GetOne(id).Result;
        //        if (model == null)
        //            Console.WriteLine("Personne introuvable.");
        //        else
        //            Console.WriteLine($"{model.FirstName} {model.LastName} {model.Age}\n");
        //    }
        //}

        //private static int GetId(string message)
        //{
        //    do
        //    {
        //        Console.Write(message);
        //        if (int.TryParse(Console.ReadLine(), out int id))
        //            return id;
        //    } while (true);
        //}
    }
}