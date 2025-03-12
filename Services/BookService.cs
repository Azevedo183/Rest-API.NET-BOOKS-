using Rest.Models;
using System.Text.Json;

public class BookService
{
    private readonly string _filePath;

    public BookService()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        _filePath = Path.Combine(basePath, "Data", "dados.json");
    }

    public List<BookModel> GetBooks()
    {
        if (!File.Exists(_filePath))
        {
            return new List<BookModel>();
        }

        var jsonData = File.ReadAllText(_filePath);
        var books = JsonSerializer.Deserialize<List<BookModel>>(jsonData);
        return books ?? new List<BookModel>();
    }
}
