using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeJustBelot.WebApi.Models
{
    public class GameModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int PlayersWaiting { get; set; }
    }
}