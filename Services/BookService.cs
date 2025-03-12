using Rest.Models;
using System.Text.Json;

namespace Rest.Services

{
    public class BookService
    {
        private readonly string _filePath = "./Data/data.json";

        public List<BookModel> GetBooks()
        {
            if (!File.Exists(_filePath)) return new List<BookModel>();

            var jsonData = File.ReadAllText(_filePath);
            var books = JsonSerializer.Deserialize<List<BookModel>>(jsonData);
            return books ?? new List<BookModel>();
        }
    }
}
