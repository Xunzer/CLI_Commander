// "Data" is the repository of the application
using System.Collections.Generic;
using CLICommander.Models;

namespace CLICommander.Data
{   
    // interface class for the application. As we are making an app with CRUD operations, the interface would imitate the CRUD functionalities with only declarations of methods
    public interface ICLICommanderRepo
    {   
        // every time you change something via dbcontext, the data won't be updated in database unless you use SaveChanges()
        bool SaveChanges();

        // return list of all command objects (GET)
        IEnumerable<Command> GetAllCommands();

        // return a single command object by provided id (GET{id})
        Command GetCommandById(int id);

        // create a Command object (POST)
        void CreateCommand(Command cmd);

        // Update a Command object (PUT)
        void UpdateCommand(Command cmd);
    }
}