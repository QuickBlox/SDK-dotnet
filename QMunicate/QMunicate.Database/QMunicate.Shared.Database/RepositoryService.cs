using System;
using System.Collections.Generic;

namespace QMunicate.Database
{
    public class RepositoryService
    {
        private static Dictionary<Type, Object> repositories = new Dictionary<Type, Object>();

        public IRepository<T> GetRepository<T>() where T : class, new()
        {
            IRepository<T> instance;
            var type = typeof(Repository<T>);
            if (repositories.ContainsKey(type))
            {
                instance = (IRepository<T>)repositories[type];
            }
            else
            {
                instance = Activator.CreateInstance<Repository<T>>();
                repositories.Add(type, instance);
            }

            return instance;
        }
    }
}
