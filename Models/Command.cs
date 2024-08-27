using System.ComponentModel.DataAnnotations;

namespace CLICommander.Models
{
    public class Command 
    {   
        // by convention, migration will know that id is primary key (not nullable by nature) so [Required] is not necessary
        [Key]
        public int Id { get; set; }

        // adding data annotations to specify value cannot be nall
        [Required]
        [MaxLength(300)] // annotation for max string length
        public string HowTo { get; set; } 
        [Required]
        public string Line { get; set; } // command line snippet that we are going to store in the database
        [Required]
        public string Platform { get; set; } // application of platform that the command line relates to
    }
}