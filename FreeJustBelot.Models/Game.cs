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
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int PlayersWaiting { get; set; }
        public Nullable<int> Player1 { get; set; }
        public Nullable<int> Player2 { get; set; }
        public Nullable<int> Player3 { get; set; }
        public Nullable<int> Player4 { get; set; }

        public virtual User User { get; set; }
    }
}
