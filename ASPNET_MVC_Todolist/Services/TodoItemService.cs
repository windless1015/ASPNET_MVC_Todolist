using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            var items = await _context.Items
                .Where(x => x.IsDone == false)
                .ToArrayAsync();
            return items;
        }

        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.NumberOfDays = newItem.NumberOfDays;

            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        //[Bind("Title,DueAt,StartFrom,NumberOfDays")] TodoItem updatedItem
        public async Task<bool> UpdateItemAsync(Guid id, TodoItem updatedItem)
        {
            //get item from id
            var item = await _context.Items
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            if (item == null) 
                return false;

            ////modify attributes
            item.Title = updatedItem.Title;
            item.StartFrom = updatedItem.StartFrom;
            item.DueAt = updatedItem.DueAt;
            item.NumberOfDays = updatedItem.NumberOfDays;

            _context.Items.Update(item);

            var saveResult = await _context.SaveChangesAsync();//save to db
            return saveResult == 1; // One entity should have been updated
        }

        public async Task<TodoItem> EditItemAsync(Guid id)
        {
            //get item from id
            var item = await _context.Items
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            return item;
        }


        public async Task<bool> MarkDoneAsync(Guid id)
        {
            var item = await _context.Items
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }

    }
}