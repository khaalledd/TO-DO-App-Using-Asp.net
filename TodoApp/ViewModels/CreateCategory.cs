using System.ComponentModel.DataAnnotations;

namespace TodoApp.ViewModels
{
    public class CreateCategory
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(4, ErrorMessage = "Name must be at least 4 characters")]
        [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters")]
        public string Name { get; set; }
    }


}
