using System.ComponentModel.DataAnnotations;

namespace TodoApp.ViewModels
{
    public class CreateTodo
    {
        public int? Id { get; set; }

        public int CategoryId { get; set; }

        [MinLength(5, ErrorMessage = "Name must be at least 4 characters")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
