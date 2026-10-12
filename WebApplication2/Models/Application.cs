using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyProfi.Models
{
    public class Application
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя студента обязательно")]
        [StringLength(100)]
        [Display(Name = "ФИО студента")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [ForeignKey("Course")]
        [Display(Name = "Курс")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required]
        [ForeignKey("Status")]
        [Display(Name = "Статус")]
        public int StatusId { get; set; } = 1;
        public StatusDict? Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}