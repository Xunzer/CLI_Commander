using System;
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

        //add a Command object to the dbcontext, need to save the changes afterward
        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _context.Commands.Add(cmd);
        }

        // whenever you make changes via dbcontext, the data won't be saved in db unless you invoke SaveChanges()
        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0); // if greater than 0, that means some entries have been successfully saved (action succeeded)
        }
    }
}