using Book.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public static List<BookData> books = new List<BookData>
        {
            new BookData
            {
                Id = Guid.NewGuid(),
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                PublishedDate = new DateTime(1925, 4, 10)
            },
            new BookData
            {
                Id = Guid.NewGuid(),
                Title = "1984",
                Author = "George Orwell",
                PublishedDate = new DateTime(1949, 6, 8)
            }
        };

        [HttpGet]
        public ActionResult<List<BookData>> GetAllBooks()
        {
            return Ok(books);
        }

        [HttpGet]
        [Route("{bookId:Guid}")]
        public ActionResult<List<BookData>> GetBookById([FromRoute] Guid bookId)
        {
            var book = books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public ActionResult<BookData> CreateBook([FromBody] BookData newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Book data is required.");
            }
            newBook.Id = Guid.NewGuid();
            books.Add(newBook);
            return CreatedAtAction(nameof(GetBookById), new { bookId = newBook.Id }, newBook);
        }

        [HttpPut]
        [Route("{bookId:Guid}")]
        public ActionResult<BookData> UpdateBook([FromRoute] Guid bookId, [FromBody] BookData updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest("Book data is required.");
            }
            var existingBook = books.FirstOrDefault(b => b.Id == bookId);
            if (existingBook == null)
            {
                return NotFound($"Book with {bookId} does not exist.");
            }
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.PublishedDate = updatedBook.PublishedDate;
            return Ok(existingBook);
        }

        [HttpDelete]
        [Route("{bookId:Guid}")]
        public ActionResult DeleteBook([FromRoute] Guid bookId)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == bookId);
            if (existingBook == null)
            {
                return NotFound($"Book with {bookId} does not exist.");
            }
            books.Remove(existingBook);
            return Ok(new {Message = $"Book with {bookId} has been deleted.", Book = existingBook});
        }
    }
}
