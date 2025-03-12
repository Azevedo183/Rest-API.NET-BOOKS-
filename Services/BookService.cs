using Rest.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rest.Services

{
    public class BookService
    {
        private readonly string _filePath = "./Data/data.json";
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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

        public async Task<BookModel?> GetBookById(int id)
        {
            if (!File.Exists(_filePath))
                return null;

            // Leitura assíncrona do arquivo
            string jsonData = await File.ReadAllTextAsync(_filePath);
            var books = JsonSerializer.Deserialize<List<BookModel>>(jsonData);
            return books?.FirstOrDefault(b => b.id == id);
        }

        public async Task<string?> GetOpenLibraryDataAsync(string title)
        {
            Console.WriteLine($"Iniciando busca por: {title}");
            string formattedTitle = title.Replace(" ", "+").ToLower();
            string url = $"https://openlibrary.org/search.json?title={formattedTitle}";
            Console.WriteLine($"URL gerada: {url}");

            try
            {
                return await _httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<string> SearchBooks(string query)
        {
            string formattedQuery = query.Replace(" ", "+").ToLower();
            string urlTitle = $"https://openlibrary.org/search.json?title={formattedQuery}";
            string urlAuthor = $"https://openlibrary.org/search.json?author={formattedQuery}";

            try
            {
                var responseTitleTask = _httpClient.GetStringAsync(urlTitle);
                var responseAuthorTask = _httpClient.GetStringAsync(urlAuthor);

                await Task.WhenAll(responseTitleTask, responseAuthorTask);

                var combinedResult = new
                {
                    TitleResults = JsonDocument.Parse(responseTitleTask.Result).RootElement,
                    AuthorResults = JsonDocument.Parse(responseAuthorTask.Result).RootElement
                };


                return JsonSerializer.Serialize(combinedResult, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (HttpRequestException)
            {
                return "{\"error\": \"Failed to fetch data from OpenLibrary\"}";
            }
        }


    }
}
