using System.Collections.Generic;
using CLICommander.Models;

// this is a mock repository and is not used in the final product, mainly for testing purpose as a template. As such, most of the methods were not completed
// "Data" is the repository of the application
namespace CLICommander.Data
{   
     // implement ICLICommanderRepo interface, which in turn assists with decoupling the code (separation of impklementation and interface)
    public class MockCLICommanderRepo : ICLICommanderRepo
    {
        public IEnumerable<Command> GetAllCommands()
        {
            // return a list of mocked Command objects back
            var commands = new List<Command>
            {   
                // mock some database data
                new Command{Id=0, HowTo="Cook a steak", Line="Heat up the pan", Platform="Cutting board & Pan"},
                new Command{Id=1, HowTo="Boil an egg", Line="Boil the water", Platform="Kettle & Pan"},
                new Command{Id=2, HowTo="Make a cup of tea", Line="Place the tea bag", Platform="Kettle & Cup"}
            };
            
            return commands;
        }

        public Command GetCommandById(int id)
        {   
            // return the mocked database data
            return new Command{Id=id, HowTo="Cook a steak", Line="Heat up the pan", Platform="Cutting board & Pan"};
        }

        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}