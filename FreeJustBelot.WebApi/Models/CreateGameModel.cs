using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FreeJustBelot.WebApi.Models
{
    [DataContract]
    public class CreateGameModel
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}