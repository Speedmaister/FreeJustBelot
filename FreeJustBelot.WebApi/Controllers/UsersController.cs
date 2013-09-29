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
    public class UsersController : BaseController
    {
        private UsersRepository repository;

        public UsersController()
        {
            var dbContext = new FreeJustBelotEntities();
            this.repository = new UsersRepository(dbContext);
        }

        public UsersController(UsersRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage Register(UserModel model)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                UsersPersister.ValidateUserRegistrationModel(model);

                var user = repository.Get(model.Username, model.Nickname, model.AuthCode);
                if (user == null)
                {
                    user = new User { Username = model.Username, AuthCode = model.AuthCode, Nickname = model.Nickname };
                }
                else
                {
                    throw new ArgumentException("User with the same username or nickname already exists.");
                }

                repository.Add(user);

                user.SessionKey = UsersPersister.GenerateSessionKey(user.Id);

                repository.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, new
                {
                    nickname = user.Nickname,
                    sessionKey = user.SessionKey
                });
            });

            return response;
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage Login(UserModel model)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                UsersPersister.ValidateUserLoginModel(model);

                var user = repository.Get(model.Username, model.AuthCode);
                if (user == null)
                {
                    throw new InvalidOperationException("Not e registered user.");
                }

                user.SessionKey = UsersPersister.GenerateSessionKey(user.Id);

                repository.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    nickname = user.Nickname,
                    sessionKey = user.SessionKey
                });
            });

            return response;
        }

        [HttpPut]
        [ActionName("logout")]
        public HttpResponseMessage Logout(LogoutModel model)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException("Null model.");
                }

                if (!UsersPersister.ValidateSessionKey(model.SessionKey))
                {
                    throw new ArgumentException("Invalid format of session key");
                }

                repository.SetSessionKeyNull(model.SessionKey);

                return Request.CreateResponse(HttpStatusCode.OK);
            });

            return response;
        }
    }
}
