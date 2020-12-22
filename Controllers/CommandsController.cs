using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _commanderRepo;
        private readonly IMapper _mapper;

        //Constructor dependencyinjection
        public CommandsController(ICommanderRepo commanderRepo, IMapper mapper)
        {
            _commanderRepo = commanderRepo;
            _mapper = mapper;
        }

        //GET: api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _commanderRepo.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //GET: api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _commanderRepo.GetComandById(id);

            if(commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }

            return NotFound();
        }

        //POST: api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto cmdCreateDto)
        {
           //Måste mappa cmdCreateDto till ett Command innan vi skickar in till databasen
           //Då det är Command databasen förstår vad det är.
            var commandModel = _mapper.Map<Command>(cmdCreateDto);

            //Skapar ett objekt med CreateCommand som finns i repo.
            _commanderRepo.CreateCommand(commandModel);

            //Methoden måste användas för att saker ska sparas till databasen.
            _commanderRepo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            //Ett rest api måste returnera både objektet som skapats och urlen för att hitta det objektet.

            //Förväntas sätta in vårat namn av metoden som returnernar ett single item för att få urien
            //Till det skapade objektet.
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        //PUT: api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto cmdUpdateDto)
        {
            var commandModelFromRepo = _commanderRepo.GetComandById(id);

            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(cmdUpdateDto, commandModelFromRepo);
            _commanderRepo.UpdateCommand(commandModelFromRepo);
            _commanderRepo.SaveChanges();

            return NoContent();
        }

        //PATCH: api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _commanderRepo.GetComandById(id);

            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);
            _commanderRepo.UpdateCommand(commandModelFromRepo);
            _commanderRepo.SaveChanges();

            return NoContent();
        }

        //DELETE: api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _commanderRepo.GetComandById(id);

            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            _commanderRepo.DeleteCommand(commandModelFromRepo);
            _commanderRepo.SaveChanges();

            return NoContent();
        }


    }
}
