﻿using Rest.Models;
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

        public BookModel? GetBookById(int id)
        {
            if (!File.Exists(_filePath)) return null;

            var jsonData = File.ReadAllText(_filePath);
            var books = JsonSerializer.Deserialize<List<BookModel>>(jsonData);
            return books?.FirstOrDefault(b => b.id == id);
        }

        public async Task<string> SearchBooks(string query)
        {
            string formattedQuery = query.Replace(" ", "+").ToLower();
            string urlTitle = $"https://openlibrary.org/search.json?title={formattedQuery}";
            string urlAuthor = $"https://openlibrary.org/search.json?author={formattedQuery}";

            try
            {
                // Faz as duas requisições em paralelo
                var responseTitleTask = _httpClient.GetStringAsync(urlTitle);
                var responseAuthorTask = _httpClient.GetStringAsync(urlAuthor);

                // Aguarda ambas completarem
                await Task.WhenAll(responseTitleTask, responseAuthorTask);

                // Combina os resultados em um JSON estruturado
                var combinedResult = new
                {
                    TitleResults = JsonDocument.Parse(responseTitleTask.Result).RootElement,
                    AuthorResults = JsonDocument.Parse(responseAuthorTask.Result).RootElement
                };

                // Retorna o JSON formatado
                return JsonSerializer.Serialize(combinedResult, new JsonSerializerOptions
                {
                    WriteIndented = true // Formatação bonita (opcional)
                });
            }
            catch (HttpRequestException)
            {
                return "{\"error\": \"Failed to fetch data from OpenLibrary\"}";
            }
        }


    }
}
