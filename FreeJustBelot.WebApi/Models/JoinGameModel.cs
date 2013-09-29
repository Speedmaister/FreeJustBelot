using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeJustBelot.WebApi.Models
{
    public class JoinGameModel
    {
        public string GameName { get; set; }

        public string Host { get; set; }

        public string Password { get; set; }
    }
}