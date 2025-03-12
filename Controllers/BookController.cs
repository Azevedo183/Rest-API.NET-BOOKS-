using Microsoft.AspNetCore.Mvc;
using Rest.Models;
using Rest.Services;

namespace Rest.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<BookModel>> Get()
        {
            return Ok(_bookService.GetBooks());
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] BookModel newBook)
        {
            if (newBook == null)
            {
                return BadRequest("O livro não pode ser nulo.");
            }

            _bookService.AddBook(newBook);
            return CreatedAtAction(nameof(Get), new { id = newBook.id }, newBook);
        }

        [HttpGet("{id}")]
        public ActionResult<BookModel> GetBook(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooksAsync([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("O parâmetro 'query' é obrigatório.");
            }

            var result = await _bookService.SearchBooks(query);
            return Content(result, "application/json");
        }
    }
}