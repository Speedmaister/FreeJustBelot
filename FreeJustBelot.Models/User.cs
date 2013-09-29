using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.Models
{
    public partial class User
    {
        public User()
        {
            this.Games = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string AuthCode { get; set; }
        public string SessionKey { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
