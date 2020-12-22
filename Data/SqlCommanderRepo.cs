using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;

namespace Commander.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext _context;

        //Dependency injecton from the DBContextclass
        public SqlCommanderRepo(CommanderContext context)
        {
            _context = context;
        }

        //Get all commands from database
        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.ToList();
        }

        //Get specifik command by id
        public Command GetComandById(int id)
        {
            return _context.Commands.FirstOrDefault(p => p.Id == id);
        }

        //POST a command to the database
        public void CreateCommand(Command cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Commands.Add(cmd);
        }

        //PUT update a command in the database
        public void UpdateCommand(Command cmd)
        {
            //NOTHING
        }


        //DELETE delete a command from the db
        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Commands.Remove(cmd);
        }


        //The data in the database won't be changed unless you call this method.
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
