using FreeJustBelot.Model;
using System;
namespace FreeJustBelot.DataLayer
{
    public interface IUsersRepository:IRepository<User>
    {
        User Get(string username, string authcode);
        User Get(string username, string nickname, string authcode);
        void SaveChanges();
        void SetSessionKeyNull(string sessionKey);
    }
}
