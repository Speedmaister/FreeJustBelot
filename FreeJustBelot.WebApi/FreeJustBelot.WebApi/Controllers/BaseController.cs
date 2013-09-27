using FreeJustBelot.DataLayer;
using FreeJustBelot.Model;
using FreeJustBelot.WebApi.Persisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FreeJustBelot.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        protected T PerformOperationAndHandleExceptions<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (ArgumentException ex)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                throw new HttpResponseException(errResponse);
            }
            catch (Exception ex)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                throw new HttpResponseException(errResponse);
            }
        }

        protected User ValidateSessionKey<T>(string sessionKey, IRepository<T> repository) where T : class
        {
            UsersPersister.ValidateSessionKey(sessionKey);
            var user = repository.GetUserBySessionKey(sessionKey);
            if (user == null)
            {
                throw new ArgumentException("Session key does not exist");
            }

            return user;
        }
    }
}
