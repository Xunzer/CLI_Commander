namespace CLICommander.Dtos
{   
    // map to the internal Command model
    public class CommandReadDto
    {   
        public int Id { get; set; }

        public string HowTo { get; set; } 

        public string Line { get; set; }

        // public string Platform { get; set; } // could be removed if we don't want to show all information to client
    }
}