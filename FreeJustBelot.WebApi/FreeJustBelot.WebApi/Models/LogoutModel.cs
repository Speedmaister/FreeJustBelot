using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FreeJustBelot.WebApi.Models
{
    [DataContract]
    public class LogoutModel
    {
        [DataMember(Name="sessionKey")]
        public string SessionKey { get; set; }
    }
}