using Microsoft.AspNetCore.Mvc;
using Rest.Models;
using Rest.Services;
using System.Net.Http;
using System.Text.Json;

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
        public async Task<IActionResult> GetBook(int id)
        {
            var localBook = await _bookService.GetBookById(id);
            if (localBook == null)
            {
                return NotFound();
            }

            var openLibraryData = await _bookService.GetOpenLibraryDataAsync(localBook.title);

            if (openLibraryData == null)
            {
                return Ok(new { LocalBook = localBook });
            }

            return Ok(new
            {
                LocalBook = localBook,
                OpenLibraryData = openLibraryData
            });
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