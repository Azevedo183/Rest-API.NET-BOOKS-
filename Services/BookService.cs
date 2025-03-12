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

        public void AddBook(BookModel newBook)
        {
            var books = GetBooks();
            newBook.id = books.Count > 0 ? books.Max(b => b.id)+1 : 1;

            books.Add(newBook);

            var updatedJson = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, updatedJson);


        }

        public BookModel? GetBookById(int id) // Renomeei para melhorar a semântica
        {
            if (!File.Exists(_filePath)) return null;

            var jsonData = File.ReadAllText(_filePath);
            var books = JsonSerializer.Deserialize<List<BookModel>>(jsonData);
            return books?.FirstOrDefault(b => b.id == id);
        }
    }
}
