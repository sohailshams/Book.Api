using Book.Api.EntityFramework;
using Book.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _dbContext;

        public BooksController(BookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookData>>> GetAllBooks()
        {
            var books = await _dbContext.Books.ToListAsync();
            return Ok(books);
        }

        [HttpGet]
        [Route("{bookId:Guid}")]
        public async Task<ActionResult<List<BookData>>> GetBookById([FromRoute] Guid bookId)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookData>> CreateBook([FromBody] BookData newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Book data is required.");
            }
            newBook.Id = Guid.NewGuid();
            _dbContext.Books.Add(newBook);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookById), new { bookId = newBook.Id }, newBook);
        }

        [HttpPut]
        [Route("{bookId:Guid}")]
        public async Task<ActionResult<BookData>> UpdateBook([FromRoute] Guid bookId, [FromBody] BookData updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest("Book data is required.");
            }
            var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (existingBook == null)
            {
                return NotFound($"Book with {bookId} does not exist.");
            }
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.PublishedDate = updatedBook.PublishedDate;
            await _dbContext.SaveChangesAsync();
            return Ok(existingBook);
        }

        [HttpDelete]
        [Route("{bookId:Guid}")]
        public async Task<ActionResult> DeleteBook([FromRoute] Guid bookId)
        {
            var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (existingBook == null)
            {
                return NotFound($"Book with {bookId} does not exist.");
            }
            _dbContext.Books.Remove(existingBook);
            await _dbContext.SaveChangesAsync();
            return Ok(new {Message = $"Book with {bookId} has been deleted.", Book = existingBook});
        }
    }
}
