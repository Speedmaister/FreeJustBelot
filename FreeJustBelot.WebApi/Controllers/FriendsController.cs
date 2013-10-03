using FreeJustBelot.Data;
using FreeJustBelot.DataLayer;
using FreeJustBelot.Models;
using FreeJustBelot.WebApi.Models;
using FreeJustBelot.WebApi.Persisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FreeJustBelot.WebApi.Controllers
{
    public class FriendsController : BaseController
    {
        private IRepository<Friend> friendsRepository;
        private IRepository<User> usersRepository;

        public FriendsController()
        {
            var dbContext = new FreeJustBelotEntities();
            this.friendsRepository = new Repository<Friend>(dbContext);
            this.usersRepository = new Repository<User>(dbContext);
        }

        [HttpGet]
        [ActionName("get-online")]
        public IEnumerable<string> GetAllFriendOnline(string sessionKey)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
               {
                   if (!UsersPersister.ValidateSessionKey(sessionKey))
                   {
                       throw new ArgumentException("Invalid format of session key");
                   }

                   var user = this.friendsRepository.GetUserBySessionKey(sessionKey);
                   var friends = this.friendsRepository.All().Where(x => x.UserId == user.Id);

                   List<string> friendsOnline = this.usersRepository.All()
                       .Where(x => x.SessionKey != null && friends.Any(y => y.FriendId == x.Id))
                       .Select(x => x.Nickname).ToList();

                   return friendsOnline;
               });

            return response;
        }

        [HttpPost]
        [ActionName("find")]
        public HttpResponseMessage FindFriend(string sessionKey, FriendModel model)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                if (!UsersPersister.ValidateSessionKey(sessionKey))
                {
                    throw new ArgumentException("Invalid format of session key");
                }

                var user = this.friendsRepository.GetUserBySessionKey(sessionKey);
                var friend = this.usersRepository.All().FirstOrDefault(x => x.Nickname == model.FriendName);

                if (friend == null)
                {
                    throw new ArgumentNullException("User does not exist.");
                }

                Friend newFriend = new Friend
                {
                    UserId = user.Id,
                    FriendId = friend.Id
                };

                this.friendsRepository.Add(newFriend);

                return Request.CreateResponse(HttpStatusCode.Created, new { Message = "Found." });
            });

            return response;
        }
    }
}