using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models
{
    public class StudentClassGrade
    {
        [Key]
        public int StudentClassGradeId { get; set; }

        public Student Student { get; set; }

        [ForeignKey("Student")]
        [Required]
        public int StudentId { get; set; }

        public Classes Classes { get; set; }

        [ForeignKey("Classes")]
        [Required]
        public int ClassId { get; set; }

        [Required]
        public double Grade { get; set; }
    }
}