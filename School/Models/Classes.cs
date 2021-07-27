using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class Classes
    {
        [Key]
        public int ClassId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string TeacherName { get; set; }
    }
}