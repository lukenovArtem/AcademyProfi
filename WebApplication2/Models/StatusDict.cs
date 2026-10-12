using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  // <-- добавьте эту строку

namespace AcademyProfi.Models
{
    [Table("StatusDict")]  // <-- добавьте эту строку
    public class StatusDict
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; } = string.Empty;
    }
}