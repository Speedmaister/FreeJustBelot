using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.Models
{
    public partial class Friend
    {
        public Friend()
        {
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }
}
