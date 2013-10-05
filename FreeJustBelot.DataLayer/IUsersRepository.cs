using FreeJustBelot.Models;
using System;
namespace FreeJustBelot.DataLayer
{
    public interface IUsersRepository:IRepository<User>
    {
        User Get(string username);
        User Get(string username, string nickname, string authcode);
        void SaveChanges();
        void SetSessionKeyNull(string sessionKey);
    }
}
