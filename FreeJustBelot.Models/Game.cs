using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.Models
{
    public partial class Game
    {
        public Game()
        {
            this.GamesRooms = new HashSet<GamesRoom>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int HostId { get; set; }
        public int PlayersWaiting { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<GamesRoom> GamesRooms { get; set; }
    }
}
