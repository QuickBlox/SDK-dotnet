using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QMunicate.Database
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Get the total objects count.
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Gets all objects from database
        /// </summary>
        IEnumerable<T> All();

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        IEnumerable<T> Filter(Expression<Func<T, Boolean>> predicate);

        /// <summary>
        /// Gets objects from database with filting and paging.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        IQueryable<T> Filter<TKey>(Expression<Func<T, Boolean>> filter, out Int32 total, Int32 index = 0,
            Int32 size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        Boolean Contains(Expression<Func<T, Boolean>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        T Find(params Object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        T Find(Expression<Func<T, Boolean>> predicate);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="value">Specified a new object to create.</param>
        T Create(T value);

        int Create(IEnumerable<T> value);
        T CreateOrReplace(T value);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="value">Specified a existing object to delete.</param>
        Int32 Delete(T value);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        Int32 Delete(Expression<Func<T, Boolean>> predicate = null);

        Int32 Delete(IEnumerable<T> items);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="value">Specified the object to save.</param>
        Int32 Update(T value);

        Int32 UpdateAll(IEnumerable<T> value);

        Int32 Insert(T value);

        Int32 InsertAll(IEnumerable<T> value);

        /// <summary>
        ///
        /// </summary>
        void Dispose();
    }
}
