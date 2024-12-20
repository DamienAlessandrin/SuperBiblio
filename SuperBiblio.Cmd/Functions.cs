﻿using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;
using System.Text;

namespace SuperBiblio.Cmd
{
    internal class Functions
    {
        // Fonctionalités du menu :
        //      - Créer un livre
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

        //      - Assigner un rayon à un livre
        public void AssignShelf(IShelfRepository shelfRepository, IBookRepository bookRepository)
        {
            ShelfModel? shelf = SelectShelf(shelfRepository, true);
            if (shelf == null)
                return;

            BookModel? book = SelectBook(bookRepository);
            if (book == null)
                return;

            book.ShelfModelId = shelf.Id;

            book = bookRepository.Update(book.Id, book).Result;
            if (book == null)
                Console.WriteLine("Livre introuvable.");
            else
                Console.WriteLine($"Le livre \"{book.Title}\" est maintenant dans le rayon {shelf.Name}\n");

            return;
        }

        //      - Lister les livres par auteur
        public void GetBooksByAuthor(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            do
            {
                int authorId = GetNumber("Id de l'auteur :");
                var author = authorRepository.Get(authorId).Result;
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

        // Lister les livres par rayon
        public void GetBooksByShelf(IBookRepository bookRepository, IShelfRepository shelfRepository)
        {
            do
            {
                ShelfModel? shelf = SelectShelf(shelfRepository, false);
                if (shelf == null)
                    return;
                else
                {
                    var books = bookRepository.GetForShelf(shelf.Id).Result;
                    StringBuilder message = new StringBuilder();
                    foreach (var book in books)
                        message.Append($"\"{book.Title}\" (Id:{book.Id})\n");

                    Console.WriteLine(message.ToString());
                    return;
                }
            }
            while (true);
        }

        //      - Rechercher des livres avec leur titre
        public void GetBooksByTitle(IBookRepository bookRepository)
        {
            string title = GetTexte("Titre du livre : ");
            var books = bookRepository.GetByTitle(title).Result;
            if (books == null)
            {
                Console.WriteLine("Erreur Api");
                return;
            }

            StringBuilder message = new StringBuilder();
            foreach (var book in books)
                if (book.Shelf != null)
                    message.Append($"\"{book.Title}\" écrit par {book.Author.FirstName} {book.Author.LastName}, rangé dans le rayon {book.Shelf.Name} (Id:{book.Id})\n");
                else
                    message.Append($"\"{book.Title}\" écrit par {book.Author.FirstName} {book.Author.LastName} (Id:{book.Id})\n");

            Console.WriteLine(message.ToString());
        }

        //      - Emprunter un livre
        public void BorrowBook(IMemberRepository memberRepository, IBookRepository bookRepository)
        {
            BookModel? book = SelectBook(bookRepository);
            if (book == null)
                return;

            if (book.Shelf != null)
            {
                book.ShelfModelId = null;

                MemberModel? member = SelectMember(memberRepository, true);
                if (member == null)
                    return;

                book.MemberModelId = member.Id;

                book = bookRepository.Update(book.Id, book).Result;
                if (book == null)
                    Console.WriteLine("Livre introuvable.");
                else
                    Console.WriteLine($"Le livre \"{book.Title}\" est maintenant emprunté par {member.FirstName} {member.LastName}\n");
            }
            else
            {
                if (book.Member != null)
                    Console.WriteLine("Ce livre est déjà emprunté.");
                else
                    Console.WriteLine("Le livre est introuvable. (ni dans un rayon, ni emprunté)");

                return;
            }
        }

        //      - Rendre un livre

        public void ReturnBorrowBook(IMemberRepository memberRepository, IBookRepository bookRepository)
        {
            MemberModel? member = SelectMember(memberRepository, false);
            if (member == null)
                return;

            var borrowBooks = bookRepository.GetForMember(member.Id).Result;

            if (borrowBooks != null && !borrowBooks.Any())
            {
                Console.WriteLine("Ce membre n'a pas emprunté de livre");
                return;
            }
            BookModel? book = SelectBorrowBookByMember(bookRepository, member);
            if (book == null)
                return;
            book.MemberModelId = null;
            bookRepository.Update(book.Id, book);
            Console.WriteLine($"Le livre {book.Title} emprunté par {member.FirstName} {member.LastName} a été rendu.");
        }

        //      - Lister les livres empruntés

        public void GetBorrowBooks(IBookRepository repository)
        {
            var books = repository.Get().Result;
            if (books == null)
            {
                Console.WriteLine("Erreur Api");
                return;
            }

            StringBuilder message = new StringBuilder();
            message.Append("*********************************************************************************************************\n");
            message.Append("Liste des livres empruntés\n");

            foreach (var book in books)
            {
                if (book.MemberModelId != null)
                {
                    message.Append($"\t- \"{book.Title}\" écrit par {book.Author.FirstName} {book.Author.LastName}, emprunté par {book.Member.FirstName} {book.Member.LastName} (Id:{book.Id})\n");
                }
            }

            message.Append("*********************************************************************************************************\n");

            Console.WriteLine(message.ToString());
        }



        // Fonctionnalité de sélection :
        //      - Sélectionner un livre
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
                {
                    var book = repository.Get(id).Result;
                    if (book != null)
                        return book;
                }

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }


        //      - Sélectionner un livre emprunté par un membre
        private static BookModel? SelectBorrowBookByMember(IBookRepository repository, MemberModel member)
        {
            do
            {
                var books = repository.GetForMember(member.Id).Result;
                if (books == null)
                {
                    Console.WriteLine("Erreur Api");
                    return null;
                }

                StringBuilder message = new StringBuilder();
                message.Append("*********************************************************************************************************\n");
                message.Append($"Liste des livres empruntés par {member.FirstName} {member.LastName}:\n");
                foreach (var book in books)
                    message.Append($"- {book.Id} : \"{book.Title}\" écrit par {book.Author.FirstName} {book.Author.LastName} (Id:{book.Id})\n");
                message.Append("\n\na : Annuler\n");
                message.Append("*********************************************************************************************************\n");
                message.Append("Option choisie : ");
                Console.Write(message.ToString());

                string reponse = Console.ReadLine();
                if (int.TryParse(reponse, out int id))
                {
                    var book = repository.Get(id).Result;
                    if (book != null && book.MemberModelId == member.Id)
                        return book;
                }

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }

        //      - Sélectionner un rayon
        private static ShelfModel? SelectShelf(IShelfRepository repository, bool NewPossible)
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

                if (NewPossible)
                    message.Append("n : Nouveau rayon\n");

                message.Append("\n\na : Annuler\n");
                message.Append("*********************************************************************************************************\n");
                message.Append("Option choisie : ");
                Console.Write(message.ToString());

                string reponse = Console.ReadLine();
                if (int.TryParse(reponse, out int id))
                {
                    var shelf = repository.Get(id).Result;
                    if (shelf != null)
                        return shelf;
                }

                if (reponse == "n" && NewPossible)
                    return CreateShelf(repository);

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }

        //      - Sélection d'un auteur
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
                {
                    var author = repository.Get(id).Result;
                    if (author != null)
                        return author;
                }

                if (reponse == "n")
                    return CreateAuthor(repository);

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }

        //      - Sélectionner un membre
        private static MemberModel? SelectMember(IMemberRepository repository, bool NewPossible)
        {
            do
            {
                var members = repository.Get().Result;
                if (members == null)
                {
                    Console.WriteLine("Erreur Api");
                    return null;
                }

                StringBuilder message = new StringBuilder();
                message.Append("*********************************************************************************************************\n");
                message.Append("Liste des membres :\n");
                foreach (var member in members)
                    message.Append($"- {member.Id} : {member.FirstName} {member.LastName} (Id:{member.Id})\n");
                if (NewPossible)
                    message.Append("n : Nouveau membre\n");
                message.Append("\n\na : Annuler\n");
                message.Append("*********************************************************************************************************\n");
                message.Append("Option choisie : ");
                Console.Write(message.ToString());

                string reponse = Console.ReadLine();
                if (int.TryParse(reponse, out int id))
                {
                    var member = repository.Get(id).Result;
                    if (member != null)
                        return member;
                }

                if (reponse == "n" && NewPossible)
                    return CreateMember(repository);

                if (reponse == "a")
                    return null;

                Console.WriteLine("Choix indisponible.");
            } while (true);
        }

        // Fonctionnalités de création (hormis le livre) :
        //      - Création d'un rayon
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

        //      - Création d'un auteur
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

        //      - Création d'un membre
        private static MemberModel? CreateMember(IMemberRepository repository)
        {
            string firstName = GetTexte("Prénom du membre : ");
            string lastName = GetTexte("Nom du membre : ");

            var member = new MemberModel() { FirstName = firstName, LastName = lastName };
            member = repository.Create(member).Result;
            if (member == null)
            {
                Console.WriteLine("Création impossible(ou erreur durant la création)");
                return null;
            }
            else
            {
                Console.WriteLine($"Nouveau membre : {member.FirstName} {member.LastName} (Id:{member.Id})");
                return member;
            }
        }

        // Fonctionnalités d'obtention :
        //      - Obtenir la liste des livres
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

        //       - Obtenir la liste des auteurs
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

        //      - Obtenir un nombre en envoyant un message
        private static int GetNumber(string message)
        {
            do
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int number))
                    return number;
            } while (true);
        }

        //      - Obtenir un texte en envoyant un message
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