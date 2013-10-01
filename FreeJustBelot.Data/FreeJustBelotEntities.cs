using FreeJustBelot.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.Data
{
    public partial class FreeJustBelotEntities : DbContext
    {
        public FreeJustBelotEntities()
            : base("name=FreeJustBelotEntities")
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
