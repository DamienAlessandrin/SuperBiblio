namespace SuperBiblio.Data.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";

        public int AuthorModelId { get; set; }
        public AuthorModel? Author { get; set; } = null!;

        public int? ShelfModelId { get; set; }
        public ShelfModel? Shelf { get; set; } = null!;

        //TODO: Jonctions (Membre, Rayon)
    }
}
