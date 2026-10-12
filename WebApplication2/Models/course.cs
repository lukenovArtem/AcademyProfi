using System.ComponentModel.DataAnnotations;

namespace AcademyProfi.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название курса обязательно")]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Цена должна быть от 0 до 1 000 000")]
        public decimal Price { get; set; }
    }
}