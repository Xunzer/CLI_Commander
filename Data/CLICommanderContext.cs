using CLICommander.Models;
using Microsoft.EntityFrameworkCore; //import EF to use ORM methods

namespace CLICommander.Data
{   
    //an instance of this class is created inside the SqlCLICommanderRepo class which is used to query database
    public class CLICommanderContext : DbContext
    {   
        // calls the base class (DbContext) constructor with given parameter
        public CLICommanderContext(DbContextOptions<CLICommanderContext> opt) : base(opt)
        {
            
        }

        // we want to represent our Command object down to our DB as a DbSet called Commands (command model creation). This maps our Command objects in the model to the database
        public DbSet<Command> Commands { get; set; } // as such "Commands" needs to be the name of table in migration
    }
}