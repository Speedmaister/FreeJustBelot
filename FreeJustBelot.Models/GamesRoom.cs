using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeJustBelot.Models
{
    public partial class GamesRoom
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Nullable<int> Player1 { get; set; }
        public Nullable<int> Player2 { get; set; }
        public Nullable<int> Player3 { get; set; }
        public Nullable<int> Player4 { get; set; }

        public virtual Game Game { get; set; }
    }
}
