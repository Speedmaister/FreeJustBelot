using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeJustBelot.WebApi.Models
{
    public class RoomModel
    {
        public List<string> Players { get; set; }

        public RoomModel()
        {
            this.Players = new List<string>();
        }
    }
}