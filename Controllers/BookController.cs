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
    }
}
