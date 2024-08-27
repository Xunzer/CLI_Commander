using System.Collections.Generic;
using AutoMapper;
using CLICommander.Data;
using CLICommander.Dtos;
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
        // we declare the interface repo here. "_" indicates private as per naming convention
        // "readonly": can be initialized either at the point of runtime, cannot be modified after initialization
        private readonly ICLICommanderRepo _repository;
        // declare the AutoMapper instance here
        private readonly IMapper _mapper;

        // dependencies are injected into "repository" and "mapper" variables
        public CommandsController(ICLICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        /* Mock repo for testing, not needed anymore
        private readonly MockCLICommanderRepo _repository = new MockCLICommanderRepo();
        */
        
        // this action will respond to "GET api/commands"
        [HttpGet] // declaring that this method will respond to GET method
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands() 
        {
            var commandItems = _repository.GetAllCommands();

            // return HTTP 200 OK result and commandItems
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems)); // map the Command type items into CommandReadDto type instances
        }

        // adding "{id}" gives us a route to this unique action result (specific command), respond to: "GET api/commands/{id}"
        [HttpGet("{id}", Name="GetCommandById")] // as this one and above method both respond to GET action (same verb), their URI must be differentiated
        public ActionResult<CommandReadDto> GetCommandById(int id) // this "id" comes from the request we pass in via the URI (Postman) by default of [ApiController] we set previously
        {
            var commandItem = _repository.GetCommandById(id); // id resolved from the request string

            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem)); // same as above
            }
            else
            {
            return NotFound(); //if the specified id doesn't exist, it will return not found
            }
        }

        // this action will respond to "POST api/commands"
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto); // map from commandCreateDto to Command type instance. Returns mapped object
            _repository.CreateCommand(commandModel); // create the model in db context
            _repository.SaveChanges();  // save the changes so that the object will be created in actual database

            // return a read dto (what we needed)
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            // by REST principle, it should also send back URI + HTTP 201 code, so we use CreatedAtRoute() here (pass back both the object and the location of where it was created)
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);

            //return Ok(commandReadDto); // will return the item and code 200
        }

        // this action will respond to "PUT api/commands/{id}". As we only return HTTP code 204, return type is "ActionResult"
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);

            // check if specified model exists, if not return 404
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            // map the newly created update model to the specified one from repo. This updates dbcontext directly
            _mapper.Map(commandUpdateDto, commandModelFromRepo);

            // still needs to call the update method although there is nothing inside the body, as some implementations may require calling it
            _repository.UpdateCommand(commandModelFromRepo);

            // save the changes in database
            _repository.SaveChanges();

            // return HTTP 204 No Content as per design
            return NoContent();
        }
    }
}