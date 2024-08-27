using System.Collections.Generic;
using System.Linq;
using CLICommander.Models;

// this is the real repository (concrete class) that we are going to use
namespace CLICommander.Data
{   
    public class SqlCLICommanderRepo : ICLICommanderRepo
    {   
        // initialize an instance of DB context class
        private readonly CLICommanderContext _context;

        // constructor injection: the dependency injection system will populate this "context" variable with data from database
        public SqlCLICommanderRepo(CLICommanderContext context)
        {
            _context = context;
        }

        // return a list of all commands from our DB
        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.ToList();
        }

        // return the command that matches the given id
        public Command GetCommandById(int id)
        {   
            // use the FirstOrDefault Linq command, it will return the first object whose id matches
            return _context.Commands.FirstOrDefault(p => p.Id == id);
        }
    }
}