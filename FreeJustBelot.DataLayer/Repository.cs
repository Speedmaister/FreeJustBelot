using FreeJustBelot.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.DataLayer
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext context;
        protected DbSet<T> set;

        public Repository(DbContext context)
        {
            this.context = context;
            set = context.Set<T>();
        }

        public virtual T Add(T item)
        {
            set.Add(item);
            context.SaveChanges();
            return item;
        }

        public T Update(int id, T item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var item = set.Find(id);
            set.Remove(item);
            context.SaveChanges();
        }

        public void Delete(T item)
        {
            set.Remove(item);
            context.SaveChanges();
        }

        public T Get(int id)
        {
            var item = set.Find(id);
            return item;
        }

        public IQueryable<T> All()
        {
            return set.AsQueryable();
        }

        public User GetUserBySessionKey(string sessionKey)
        {
            var user = this.context.Set<User>()
                .FirstOrDefault(x => x.SessionKey == sessionKey);
            return user;
        }
    }
}
