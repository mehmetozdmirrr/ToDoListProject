using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.Models
{
    public class Note
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? userId { get; set; }
        public User? user { get; set; } // Navigation property
        public bool IsDeleted { get; set; } 
    }
}
