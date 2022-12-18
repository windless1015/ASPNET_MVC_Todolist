using Microsoft.AspNetCore.Mvc;
using TodoItemMVC.Services;
using TodoItemMVC.Models;

namespace TodoItemMVC.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> Index()
        {
            // Get to-do items from backend api
            var items = await _todoItemService.GetIncompleteItemsAsync();

            // Put items into a model
            var model = new TodoViewModel()
            {
                Items = items
            };

            // Render view using the model
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            string retString = CheckInputDateValidation(newItem);
            if (retString != "OK")
            {
                return Content(retString, "text/html");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.AddItemAsync(newItem);
            if (!successful)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.MarkDoneAsync(id);
            if (!successful)
            {
                return BadRequest("Could not mark item as done.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var retItem = await _todoItemService.EditItemAsync(id);

            return View(retItem);
        }

        public async Task<IActionResult> UpdateItem(Guid id, TodoItem updatedItem)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            string retString = CheckInputDateValidation(updatedItem);
            if (retString != "OK")
            {
                return Content(retString, "text/html");
            }

            var retItem = await _todoItemService.UpdateItemAsync(id, updatedItem);

            return RedirectToAction("Index");
        }

        private bool CheckNumberOfDays(DateTimeOffset? start, DateTimeOffset? end, int? numberOfDays)
        {
            // both start and due date has value, then check the numberOfDays is meet the requirement of these two dates
            if (start.HasValue && end.HasValue && numberOfDays.HasValue)
            {
                TimeSpan ts = (TimeSpan)(end - start);
                if (numberOfDays <= Convert.ToInt32(ts.TotalDays))
                {
                    return true;
                }
                else
                    return false;
            }
            //this because use has not set the specified start date and due date, so the number of days
            // can set any value
            return true;
        }

        private string CheckInputDateValidation(TodoItem item)
        {
            //check start date should not be before today
            //if (item.StartFrom.HasValue)
            //{
            //    if (item.StartFrom.Value < DateTime.Now.Date)
            //    {
            //        return "<script>alert('Please make sure the start date is not before today!');history.go(-1);</script>";
            //    }
            //}
            //check start date should be later than due date
            if (item.StartFrom.HasValue && item.DueAt.HasValue)
            {
                //TimeSpan ts = newItem.StartFrom - newItem.DueAt;
                int diff = DateTimeOffset.Compare(item.StartFrom.Value, item.DueAt.Value);
                if (diff >= 0) //start is later or equal of due
                {
                    return "<script>alert('Please make sure the start date is before due date!');history.go(-1);</script>";
                }
            }

            //check if the numberOfDays is reasonable for StartFrom and DueAt
            bool hasSetNumberOfDays = CheckNumberOfDays(item.StartFrom, item.DueAt, item.NumberOfDays);
            if (!hasSetNumberOfDays) // numberOfDays > due  - start
            {
                return "<script>alert('NumberofDays has exceeded the allowed rang!');history.go(-1);</script>";
            }

            return "OK";
        }


    }
}
