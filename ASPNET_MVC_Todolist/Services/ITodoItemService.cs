using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync();

        Task<bool> AddItemAsync(TodoItem newItem);

        Task<TodoItem> EditItemAsync(Guid id);

        Task<bool> UpdateItemAsync(Guid id, TodoItem updatedItem);

        Task<bool> MarkDoneAsync(Guid id);
    }
}