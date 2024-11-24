using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;
using System.Text;

namespace SuperBiblio.Cmd
{
    internal class Functions
    {
        //TODO: Fonction pour ajouter un livre à un auteur (créer un livre en choisissant son auteur)
        //TODO: Fonction pour créer un auteur ou ajouter un choix dans la liste des auterus pour créer l'auteur s'il n'existe pas
        // FuncCreateBook --> Choose title
        //                --> Select Author --> FuncSelectAuthor --> Select author existing
        //                                                       --> Select new author (last option) --> FuncCreateAuthor --> Choose FirstName
        //                                                                                                                --> Choose LastName (optionnal)

        public void GetBooks(IBookRepository repository)
        {
            var books = repository.Get().Result;
            if (books == null)
            {
                Console.WriteLine("Erreur Api");
                return;
            }

            StringBuilder message = new StringBuilder();
            foreach (var book in books)
                message.Append($"\"{book.Title}\" écrit par {book.Author.FirstName} {book.Author.LastName} (Id:{book.Id})\n");

            Console.WriteLine(message.ToString());
        }

        public void GetAuthors(IAuthorRepository repository)
        {
            var authors = repository.Get().Result;
            if (authors == null)
            {
                Console.WriteLine("Erreur Api");
                return;
            }

            StringBuilder message = new StringBuilder();
            foreach (var author in authors)
                message.Append($"{author.FirstName} {author.LastName} (Id:{author.Id})\n");

            Console.WriteLine(message.ToString());
        }

        public void CreateBook(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            string titre = GetTexte("Titre du livre : ");

            AuthorModel? author = SelectAuthor(authorRepository);
            if (author == null)
            {
                Console.WriteLine("error");
                return;
            }

            var book = new BookModel() { Title = titre, AuthorModelId = author.Id };
            book = bookRepository.Create(book).Result;
            if (book == null)
                Console.WriteLine("Création impossible(ou erreur durant la création)\n");
            else
                Console.WriteLine($"Nouveau livre : {book.Title} écrit par {author.FirstName} {author.LastName} (Id:{book.Id})\n");
        }

        // SelectAuthor ou ChooseAuthor
        private static AuthorModel? SelectAuthor(IAuthorRepository repository)
        {
            do
            {
                var authors = repository.Get().Result;
                if (authors == null)
                {
                    Console.WriteLine("Erreur Api");
                    return null;
                }

                StringBuilder message = new StringBuilder();
                message.Append("*********************************************************************************************************\n");
                message.Append("Liste des auteurs :\n");
                foreach (var author in authors)
                    message.Append($"- {author.Id} : {author.FirstName} {author.LastName} (Id:{author.Id})\n");
                message.Append("n : Nouvel auteur\n");

                //message.Append("\n\nq : Quitter"); // TODO: Peut on quitter dans cette phase (ou dans toutes les phases, ou jamais, ou que pour la création du livre) (plutot remplacer par précédent)
                message.Append("*********************************************************************************************************\n");
                message.Append("Option choisie : ");
                Console.Write(message.ToString());

                string reponse = Console.ReadLine();
                if (int.TryParse(reponse, out int id))
                    return repository.Get(id).Result;

                if (reponse == "n")
                    CreateAuthor(repository); // TODO: voir pour retourner directement l'auteur qui vient d'être créer

                //if (reponse == "q")
                //    return null;
            } while (true);
        }

        private static void CreateAuthor(IAuthorRepository repository)
        {
            string firstName = GetTexte("Prénom de l'auteur ou nom d'auteur : ");
            string lastName = GetTexte("Nom de l'auteur : ");

            var author = new AuthorModel() { FirstName = firstName, LastName = lastName };
            author = repository.Create(author).Result;
            if (author == null)
                Console.WriteLine("Création impossible(ou erreur durant la création)");
            else
                Console.WriteLine($"Nouvel auteur : {author.FirstName} {author.LastName} (Id:{author.Id})");
        }

        private static int GetNumber(string message)
        {
            do
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int number))
                    return number;
            } while (true);
        }

        private static string GetTexte(string message)
        {
            do
            {
                Console.Write(message);
                string texte = Console.ReadLine();
                if (texte != null)
                    return texte;
            } while (true);
        }
    }
}