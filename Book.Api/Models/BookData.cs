namespace Book.Api.Models
{
    public class BookData
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int MyProperty { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
