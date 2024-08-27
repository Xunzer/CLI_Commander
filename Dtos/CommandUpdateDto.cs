using System.ComponentModel.DataAnnotations;

namespace CLICommander.Dtos
{   
    // map to the internal Command model, a separate update DTO is also beneficial for potential future building
    public class CommandUpdateDto
    {   
        // public int Id { get; set; } // no need to keep Id because id can never be altered (as the primary key)
        // for updates, we could only possibly alter the following fields in the database since these are the only updatable ones

        [Required]
        [MaxLength(300)]
        public string HowTo { get; set; }
        
        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }
    }
}