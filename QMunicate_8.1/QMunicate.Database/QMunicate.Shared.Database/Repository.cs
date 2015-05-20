using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QMunicate.Database
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        #region Ctor

        /// <summary>
        ///
        /// </summary>
        public Repository()
        {
            Database = new Database(StorageType.Local);
            Database.Connection.CreateTable<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        private Database Database { get; set; }

        #endregion

        #region IRepository Members

        /// <summary>
        /// Get the total objects count.
        /// </summary>
        public Int32 Count
        {
            get { return Database.Connection.Table<T>().Count(); }
        }

        /// <summary>
        /// Gets all objects from database
        /// </summary>
        public IEnumerable<T> All()
        {
            return Database.Connection.Table<T>();
        }

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        public IEnumerable<T> Filter(Expression<Func<T, Boolean>> predicate)
        {
            return Database.Connection.Table<T>().Where(predicate);
        }

        /// <summary>
        /// Gets objects from database with filting and paging.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        public IQueryable<T> Filter<TKey>(Expression<Func<T, Boolean>> filter, out Int32 total, Int32 index = 0,
            Int32 size = 50)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        public Boolean Contains(Expression<Func<T, Boolean>> predicate)
        {
            return Database.Connection.Find(predicate) != null;
        }

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        public T Find(params Object[] keys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        public T Find(Expression<Func<T, Boolean>> predicate)
        {
            return Database.Connection.Find(predicate);
        }

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="value">Specified a new object to create.</param>
        public T Create(T value)
        {
            Database.Connection.Insert(value);
            return value;
        }

        public int Create(IEnumerable<T> value)
        {
            return Database.Connection.InsertAll(value);
        }

        public T CreateOrReplace(T value)
        {
            Database.Connection.InsertOrReplace(value);
            return value;
        }

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="value">Specified a existing object to delete.</param>
        public Int32 Delete(T value)
        {
            return Database.Connection.Delete(value);
        }

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        public Int32 Delete(Expression<Func<T, Boolean>> predicate = null)
        {
            var count = 0;
            if (predicate != null)
            {
                var foundedItems = Database.Connection.Table<T>().Where(predicate).ToList();
                count = Database.Connection.Delete(foundedItems);
            }
            else
            {
                count = Database.Connection.DeleteAll<T>();
            }

            return count;
        }

        public Int32 Delete(IEnumerable<T> items)
        {
            return Database.Connection.Delete(items);
        }

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="value">Specified the object to save.</param>
        public Int32 Update(T value)
        {
            return Database.Connection.Update(value);
        }

        public Int32 UpdateAll(IEnumerable<T> value)
        {
            return Database.Connection.UpdateAll(value);
        }

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="value">Specified the object to save.</param>
        public Int32 Insert(T value)
        {
            return Database.Connection.Insert(value);
        }

        public Int32 InsertAll(IEnumerable<T> value)
        {
            return Database.Connection.InsertAll(value);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Database.Connection.Dispose();
        }

        #endregion
    }
}
