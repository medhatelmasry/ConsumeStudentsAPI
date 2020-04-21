using System.ComponentModel.DataAnnotations;

namespace ConsumeStudentsAPI.Models {
    public class Student {

        [Display (Name = "ID")]
        [Key]
        public string studentId { get; set; }

        [Required]
        [Display (Name = "First Name")]
        public string firstName { get; set; }

        [Required]
        [Display (Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display (Name = "School")]
        public string school { get; set; }
    }
}