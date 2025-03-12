using Microsoft.AspNetCore.Http;
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

        public BookController()
        {
            _bookService = new BookService();
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
        public ActionResult<List<BookModel>> GetBook(int id)
        {
            return Ok(_bookService.GetBookById(id));
        }
    }
}
