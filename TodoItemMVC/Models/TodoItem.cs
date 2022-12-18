using System.ComponentModel.DataAnnotations;

namespace TodoItemMVC.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public bool IsDone { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTimeOffset? DueAt { get; set; }

        public DateTimeOffset? StartFrom { get; set; }

        public int? NumberOfDays { get; set; }
    }
}
