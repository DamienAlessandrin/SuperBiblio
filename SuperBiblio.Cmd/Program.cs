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
                menu.Append("- 2 : Assigner un rayon à un livre.\n");

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

        ////TODO: Fonction pour ajouter un livre à un auteur (créer un livre en choisissant son auteur)
        ////TODO: Fonction pour créer un auteur ou ajouter un choix dans la liste des auterus pour créer l'auteur s'il n'existe pas
        //// FuncCreateBook --> Choose title
        ////                --> Select Author --> FuncSelectAuthor --> Select author existing
        ////                                                       --> Select new author (last option) --> FuncCreateAuthor --> Choose FirstName
        ////                                                                                                                --> Choose LastName (optionnal)

        //private static void GetBooks(IBookRepository repository)
        //{
        //    var books = repository.Get().Result;
        //    if (books == null)
        //    {
        //        Console.WriteLine("Erreur Api");
        //        return;
        //    }

        //    StringBuilder message = new StringBuilder();
        //    foreach (var book in books)
        //        message.Append($"\"{book.Title}\" écrit par {book.Author.FirstName} {book.Author.LastName} (Id:{book.Id})\n");

        //    Console.WriteLine(message.ToString());
        //}

        //private static void GetAuthors(IAuthorRepository repository)
        //{
        //    var authors = repository.Get().Result;
        //    if (authors == null)
        //    {
        //        Console.WriteLine("Erreur Api");
        //        return;
        //    }

        //    StringBuilder message = new StringBuilder();
        //    foreach (var author in authors)
        //        message.Append($"{author.FirstName} {author.LastName} (Id:{author.Id})\n");

        //    Console.WriteLine(message.ToString());
        //}

        //private static void CreateBook(IBookRepository bookRepository, IAuthorRepository authorRepository)
        //{
        //    AuthorModel? author = SelectAuthor(authorRepository);
        //    if (author == null)
        //    {
        //        Console.WriteLine("error");
        //        return;
        //    }

        //    var book = new BookModel() { Id = 4, Title = "Les misérables", AuthorModelId = author.Id };
        //    book = bookRepository.Create(book).Result;
        //    if (book == null)
        //        Console.WriteLine("Création impossible(ou erreur durant la création)");
        //    else
        //        Console.WriteLine($"Nouveau livre : {book.Title} écrit par {author.FirstName} {author.LastName} (Id:{book.Id})");
        //}

        //// SelectAuthor ou ChooseAuthor
        //private static AuthorModel? SelectAuthor(IAuthorRepository repository)
        //{
        //    string reponse;
        //    do
        //    {
        //        var authors = repository.Get().Result;
        //        if (authors == null)
        //        {
        //            Console.WriteLine("Erreur Api");
        //            return null;
        //        }

        //        StringBuilder message = new StringBuilder();
        //        message.Append("*********************************************************************************************************\n");
        //        message.Append("Liste des auteurs :\n");
        //        foreach (var author in authors)
        //            message.Append($"- {author.Id} : {author.FirstName} {author.LastName} (Id:{author.Id})\n");
        //        message.Append("n : Nouvel auteur\n");

        //        //message.Append("\n\nq : Quitter"); // TODO: Peut on quitter dans cette phase (ou dans toutes les phases, ou jamais, ou que pour la création du livre) (plutot remplacer par précédent)
        //        message.Append("*********************************************************************************************************\n");
        //        message.Append("Option choisie : ");
        //        Console.Write(message.ToString());

        //        reponse = Console.ReadLine();
        //        if (int.TryParse(reponse, out int id))
        //            return repository.Get(id).Result;

        //        if (reponse == "n")
        //            CreateAuthor(repository); // TODO: voir pour retourner directement l'auteur qui vient d'être créer

        //        //if (reponse != "q")
        //        //    return null;
        //    } while (true);
        //}

        //private static void CreateAuthor(IAuthorRepository repository)
        //{
        //    var author = new AuthorModel() { FirstName = "Victor", LastName = "Hugo", Id = 4 };
        //    author = repository.Create(author).Result;
        //    if (author == null)
        //        Console.WriteLine("Création impossible(ou erreur durant la création)");
        //    else
        //        Console.WriteLine($"Nouvel auteur : {author.FirstName} {author.LastName} (Id:{author.Id})");
        //}

        ////private static void GetPerson(IPersonRepository repository)
        ////{
        ////    while (true)
        ////    {
        ////        int id = GetId("id de la personne : ");
        ////        if (id == 0)
        ////            return;

        ////        var model = repository.GetOne(id).Result;
        ////        if (model == null)
        ////            Console.WriteLine("Personne introuvable.");
        ////        else
        ////            Console.WriteLine($"{model.FirstName} {model.LastName} {model.Age}\n");
        ////    }
        ////}

        ////private static int GetId(string message)
        ////{
        ////    do
        ////    {
        ////        Console.Write(message);
        ////        if (int.TryParse(Console.ReadLine(), out int id))
        ////            return id;
        ////    } while (true);
        ////}

        ////        string lastName;
        ////            do
        ////            {
        ////                Console.Write("Nouveau nom de la personne : ");
        ////                lastName = Console.ReadLine();
        ////                if (lastName != null)
        ////                    break;
        ////            } while (true);
        ////            person.LastName = lastName;
        ////            //TODO: fonction pour demander le nom de la personne

        ////            string firstName;
        ////            do
        ////            {
        ////                Console.Write("Nouveau prénom de la personne : ");
        ////                firstName = Console.ReadLine();
        ////                if (firstName != null)
        ////                    break;
        ////            } while (true) ;
        ////person.FirstName = firstName;
        //////TODO: fonction pour demander le prénom de la personne

        ////int age;
        ////do
        ////{
        ////    Console.Write("Nouvel age de la personne : ");
        ////    if (int.TryParse(Console.ReadLine(), out age))
        ////        break;
        ////} while (true);
        ////person.Age = age;
        ////            //TODO: fonction pour demander l'age de la personne
    }
}