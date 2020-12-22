 using System;
using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class CommanderContext : DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> opt) : base(opt)
        {

        }

        //We want to represent our command object down to our DataBase as a DBset
        //We need this to map down to our Database
        public DbSet<Command> Commands { get; set; }
    }
}
