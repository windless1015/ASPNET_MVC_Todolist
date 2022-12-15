using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            /*
             {
              "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
              "isDone": true,
              "title": "string",
              "dueAt": "2022-12-15T14:41:07.147Z",
              "startFrom": "2022-12-15T14:41:07.147Z",
              "numberOfDays": 0
            }
             */


            //var items = await _context.Items
            //    .Where(x => x.IsDone == false)
            //    .ToArrayAsync();
            //return items;
            TodoItem[] a = new TodoItem[0];
            
            return a;
        }

        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.NumberOfDays = newItem.NumberOfDays;

            //string json = "{\"id\":\"" + newItem.Id.ToString() + "\",\"isDone\":false,\"title\":\"" + newItem.Title + "\",\"numberOfDays\":"+ newItem.NumberOfDays.ToString()+ "}";
            //TodoItem test = JsonConvert.DeserializeObject<TodoItem>(json);

            string jsonString =  JsonConvert.SerializeObject(newItem);
            // Create a request to the specified URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:7194/api/TodoItems");
            request.Method = "POST";
            request.ContentType = "application/json";
            // Write the JSON data to the request stream
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonString);
            }

            // Send the request and get the response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;

            // Read the response as a string
            //using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            //{
            //    responseString = reader.ReadToEnd();
            //}

            //var saveResult = await _context.SaveChangesAsync();
            //return saveResult == 1;
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