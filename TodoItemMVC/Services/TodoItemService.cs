using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Text.Json;
using System.Text;
using TodoItemMVC.Services;
using TodoItemMVC.Models;

namespace TodoItemMVC.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly HttpClient _httpClient;
        private string baseUrl = "https://localhost:7194";
        public TodoItemService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl + "/api/TodoItems");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<TodoItem[]>();
                return content;
            }
            return null;
        }

        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;

            var todoItemJson = new StringContent(JsonSerializer.Serialize(newItem), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await _httpClient.PostAsync(baseUrl + "/api/TodoItems", todoItemJson);
            httpResponseMessage.EnsureSuccessStatusCode();
            if (httpResponseMessage.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }

        //[Bind("Title,DueAt,StartFrom,NumberOfDays")] TodoItem updatedItem
        public async Task<bool> UpdateItemAsync(Guid id, TodoItem updatedItem)
        {
            var todoItemJson = new StringContent(JsonSerializer.Serialize(updatedItem), Encoding.UTF8, Application.Json);
            using var httpResponseMessage =
                await _httpClient.PutAsync(baseUrl +  $"/api/TodoItems/{updatedItem.Id}", todoItemJson);
            httpResponseMessage.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<TodoItem> EditItemAsync(Guid id)
        {
            //get item from id and then fill the TodoItem properties into the edit page
            var response = await _httpClient.GetAsync(baseUrl + $"/api/TodoItems/{id}");
            if (response.IsSuccessStatusCode)
            {
               var item = await response.Content.ReadFromJsonAsync<TodoItem>();
                return item;
            }
            return new TodoItem();
        }


        public async Task<bool> MarkDoneAsync(Guid id)
        {
            var response = await _httpClient.GetAsync(baseUrl + $"/api/TodoItems/{id}");
            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<TodoItem>();
                if (item == null) 
                    return false;
                var todoItem = item as TodoItem;
                todoItem.IsDone = true;

                var todoItemJson = new StringContent(JsonSerializer.Serialize(todoItem), Encoding.UTF8, Application.Json);

                using var httpResponseMessage = await _httpClient.PutAsync(baseUrl + $"/api/TodoItems/{todoItem.Id}", todoItemJson);
                httpResponseMessage.EnsureSuccessStatusCode();
                return true;
            }
            return false;
        }
    }
}
