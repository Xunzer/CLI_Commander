// "Data" is the repository of the application
using System.Collections.Generic;
using CLICommander.Models;

namespace CLICommander.Data
{   
    // interface class for the application. As we are making an app with CRUD operations, the interface would imitate the CRUD functionalities with only declarations of methods
    public interface ICLICommanderRepo
    {   
        // return list of all command objects
        IEnumerable<Command> GetAllCommands();

        // return a single command object by provided id
        Command GetCommandById(int id);
    }
}