using System.ComponentModel.DataAnnotations;

namespace CLICommander.Dtos
{   
    // map to the internal Command model
    public class CommandCreateDto
    {   
        // public int Id { get; set; } // no need to keep Id because db will create the id number for the item

         // data annotations here will require these during creation. For any errors, return 400 Bad Request instead of 500 to client
        [Required]
        [MaxLength(300)]
        public string HowTo { get; set; }
        
        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; } // platform is required when creating 
    }
}