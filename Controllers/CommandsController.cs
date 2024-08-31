using System.Collections.Generic;
using AutoMapper;
using CLICommander.Data;
using CLICommander.Dtos;
using CLICommander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get all available commands.")] // Swagger annotation for the UI
        [HttpGet] // declaring that this method will respond to GET method
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands() 
        {
            var commandItems = _repository.GetAllCommands();

            // return HTTP 200 OK result and commandItems
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems)); // map the Command type items into CommandReadDto type instances
        }

        // adding "{id}" gives us a route to this unique action result (specific command), respond to: "GET api/commands/{id}"
        [SwaggerOperation(Summary = "Get specified command by id.")]
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
        [SwaggerOperation(Summary = "Create a command. Please provide following attributes: \"howTo\" is the description of command, \"line\" is the code of the command, and \"platform\" is the application platform of the command.")]
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
        [SwaggerOperation(Summary = "Replace the content of specified command by id with content you provide.")]
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);

            // check if specified model exists, if not return 404
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            // map the newly created update dto model to the specified one from repo. This updates dbcontext directly
            _mapper.Map(commandUpdateDto, commandModelFromRepo);

            // still needs to call the update method although there is nothing inside the body, as some implementations may require calling it
            _repository.UpdateCommand(commandModelFromRepo);

            // save the changes in database
            _repository.SaveChanges();

            // return HTTP 204 No Content as per design
            return NoContent();
        }

        // this action will respond to "PATCH api/commands/{id}". The JSON Patch document will operate on "CommandUpdateDto" type object
        [SwaggerOperation(Summary = "Partially replace the content of the command. \"op\" is the operation you want to perform, \"path\" is the name of attributes and \"value\" is the new content you want for it. Note for \"path\" you need to add a \"/\" in front of the specified attribute, replace \"from\" with \"value\" and specify its content.")]
        [HttpPatch("{id}")]
        public ActionResult PartialUpdateCommand(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {   
            var commandModelFromRepo = _repository.GetCommandById(id);
                        
            // check if specified model exists, if not return 404
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            // as we have the patch document from client, we need to add the patch to the specified object. Here we map the specified Command object to an update dto
            var commandForPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);

            // apply the patch. Make sure the object conforms to validations
            patchDoc.ApplyTo(commandForPatch, ModelState);

            // validation check for the object
            if(!TryValidateModel(commandForPatch))
            {   
                // if validation fails, the code returns a ValidationProblem with the current ModelState, which contains all the validation errors
                return ValidationProblem(ModelState);
            }

            // map the newly patched dto model to the specified one from repo. This updates dbcontext directly
            _mapper.Map(commandForPatch, commandModelFromRepo);

            // redundant empty update command called per standard then save the changes
            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            // return HTTP 204 No Content as per design
            return NoContent();
        }

        // this action will respond to "DELETE api/commands/{id}"
        [SwaggerOperation(Summary = "Delete the specified command by id.")]
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);

            // check if specified model exists, if not return 404
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            
            // delete the specified model and save the changes
            _repository.DeleteCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }
    }
}