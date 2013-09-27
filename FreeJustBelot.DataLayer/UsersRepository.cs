using FreeJustBelot.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.DataLayer
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(DbContext context)
            : base(context)
        {
        }

        public User Get(string username, string authcode)
        {
            var user = this.set.FirstOrDefault(x => x.Username == username && x.AuthCode == authcode);
            return user;
        }

        public User Get(string username, string nickname, string authcode)
        {
            var user = this.set.FirstOrDefault(x => x.Username == username && x.AuthCode == authcode && x.Nickname == nickname);
            return user;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void SetSessionKeyNull(string sessionKey)
        {
            var user = this.set.FirstOrDefault(x => x.SessionKey == sessionKey);
            if (user == null)
            {
                throw new ArgumentException("User with this session key does not exist");
            }

            user.SessionKey = null;

            context.SaveChanges();
        }
    }
}
