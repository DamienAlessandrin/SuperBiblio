using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;
using System.Text;

namespace SuperBiblio.Cmd
{
    internal class Functions
    {
        //TODO: Fonction pour assigner un rayon à un livre
        // FuncAssignShelf --> Select Shelf --> FuncSelectShelf --> Select shelf
        //                                                      --> Select new shelf (last option) --> FuncCreatShelf --> Choose Name
        //                 --> Select Book  --> FuncSelectBook  --> Select Book
        //
        // TODO: Créer une fonction qui retourne un menu de sélection si on lui donne une liste d'éléments (getAll) ou un repository (repository.getAll() dans le select)

        public void AssignShelf(IShelfRepository shelfRepository, IBookRepository bookRepository)
        {
            ShelfModel? shelf = SelectShelf(shelfRepository);
            if (shelf != null)
                return;

            BookModel? book = SelectBook(bookRepository);
            if (book != null)
                return;



        }

        private static BookModel? SelectBook(IBookRepository repository)
        {
            do
            {
                var books = repository.Get().Result;
                if (books == null)
                {
                    Console.WriteLine("Erreur Api");
                    return null;
                }

                StringBuilder message = new StringBuilder();
                message.Append("*********************************************************************************************************\n");
                message.Append("Liste des Livres :\n");
                foreach (var book in books)
                    message.Append($"- {book.Id} : \"{book.Title}\" écrit par {book.Author.FirstName} {book.Author.LastName} (Id:{book.Id})\n");
                message.Append("\n\na : Annuler\n");
                message.Append("*********************************************************************************************************\n");
                message.Append("Option choisie : ");
                Console.Write(message.ToString());

                string reponse = Console.ReadLine();
                if (int.TryParse(reponse, out int id))
                    return repository.Get(id).Result;

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }

        private static ShelfModel? SelectShelf(IShelfRepository repository)
        {
            do
            {
                var shelves = repository.Get().Result;
                if (shelves == null)
                {
                    Console.WriteLine("Erreur Api");
                    return null;
                }

                StringBuilder message = new StringBuilder();
                message.Append("*********************************************************************************************************\n");
                message.Append("Liste des rayons :\n");
                foreach (var shelf in shelves)
                    message.Append($"- {shelf.Id} : {shelf.Name} (Id:{shelf.Id})\n");
                message.Append("n : Nouveau rayon\n");
                message.Append("\n\na : Annuler\n");
                message.Append("*********************************************************************************************************\n");
                message.Append("Option choisie : ");
                Console.Write(message.ToString());

                string reponse = Console.ReadLine();
                if (int.TryParse(reponse, out int id))
                    return repository.Get(id).Result;

                if (reponse == "n")
                    return CreateShelf(repository);

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }

        private static ShelfModel? CreateShelf(IShelfRepository repository)
        {
            string shelfName = GetTexte("Nom du rayon : ");

            var shelf = new ShelfModel() { Name = shelfName };
            shelf = repository.Create(shelf).Result;
            if (shelf == null)
            {
                Console.WriteLine("Création impossible (ou erreur durant la création)");
                return null;
            }
            else
            {
                Console.WriteLine($"Nouveau rayon : {shelf.Name} (Id:{shelf.Id})");
                return shelf;
            }
        }

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

        public void GetBooksByAuthor(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            do
            {
                int authorId = GetNumber("Id de l'auteur :");
                var author = authorRepository.Get(authorId).Result ;
                if (author == null)
                {
                    Console.WriteLine($"L'auteur {authorId} n'existe pas");
                }
                else
                {
                    var books = bookRepository.GetForAuthor(authorId).Result;
                    StringBuilder message = new StringBuilder();
                    foreach (var book in books)
                        message.Append($"\"{book.Title}\" (Id:{book.Id})\n");

                    Console.WriteLine(message.ToString());
                    return;
                }
            }
            while (true);
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
                return;

            var book = new BookModel() { Title = titre, AuthorModelId = author.Id };
            book = bookRepository.Create(book).Result;
            if (book == null)
                Console.WriteLine("Création impossible(ou erreur durant la création)\n");
            else
                Console.WriteLine($"Nouveau livre : {book.Title} écrit par {author.FirstName} {author.LastName} (Id:{book.Id})\n");
        }

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
                message.Append("\n\na : Annuler\n");
                message.Append("*********************************************************************************************************\n");
                message.Append("Option choisie : ");
                Console.Write(message.ToString());

                string reponse = Console.ReadLine();
                if (int.TryParse(reponse, out int id))
                    return repository.Get(id).Result;

                if (reponse == "n")
                    return CreateAuthor(repository);

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }

        private static AuthorModel? CreateAuthor(IAuthorRepository repository)
        {
            string firstName = GetTexte("Prénom de l'auteur ou nom d'auteur : ");
            string lastName = GetTexte("Nom de l'auteur : ");

            var author = new AuthorModel() { FirstName = firstName, LastName = lastName };
            author = repository.Create(author).Result;
            if (author == null)
            {
                Console.WriteLine("Création impossible(ou erreur durant la création)");
                return null;
            }
            else
            {
                Console.WriteLine($"Nouvel auteur : {author.FirstName} {author.LastName} (Id:{author.Id})");
                return author;
            }
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