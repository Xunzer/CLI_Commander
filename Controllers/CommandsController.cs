using System.Collections.Generic;
using CLICommander.Data;
using CLICommander.Models;
using Microsoft.AspNetCore.Mvc;

// for decoupling purpose: we use this controller class to access the repository's implemented class and then that class would access the database
namespace CLICommander.Controllers
{   
    // [] are attributes which are declarative tags to give runtime information for the whole class
    // controller level route (base route): how you get to resources/api endpoints
    // Route("..."): matches URI to an action --- will use the routing path from actions
    [Route("api/commands")]
    [ApiController] //gives some extra behaviours (makes life easier)
    public class CommandsController : ControllerBase // ControllerBase is enough since Controller itself would bring in view support
    {
        private readonly ICLICommanderRepo _repository;

        public CommandsController(ICLICommanderRepo repository)
        {
            _repository = repository;
        }


        // we declare the interface repo here. "_" indicates private as per naming convention
        // "readonly": can be initialized either at the point of runtime, cannot be modified after initialization

        /* Mock repo for testing, not needed anymore
        private readonly MockCLICommanderRepo _repository = new MockCLICommanderRepo();
        */
        
        // this action will respond to "GET api/commands"
        [HttpGet] // declaring that this method will respond to GET method
        public ActionResult<IEnumerable<Command>> GetAllCommands() 
        {
            var commandItems = _repository.GetAllCommands();

            // return HTTP 200 OK result and commandItems
            return Ok(commandItems);
        }

        // adding "{id}" gives us a route to this unique action result (specific command), respond to: "GET api/commands/id"
        [HttpGet("{id}")] // as this one and above method both respond to GET action (same verb), their URI must be differentiated
        public ActionResult<Command> GetCommandById(int id) // this "id" comes from the request we pass in via the URI (Postman) by default of [ApiController] we set previously
        {
            var commandItem = _repository.GetCommandById(id); // id resolved from the request string

            return Ok(commandItem);
        }
    }
}