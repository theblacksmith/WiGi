namespace WiGi
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Domain;

	public interface IScoped<TEntity> where TEntity : class
	{
		void AddScope(string name, Expression<Func<TEntity, bool>> condition);
		void AddScope(string name, Expression<Func<TEntity, bool>> condition, bool permanent);
		bool RemoveScope(string name);
	}

	public interface IRepository<TEntity> : IDisposable, IScoped<TEntity> where TEntity : class
	{
		IEnumerable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
		IEnumerable<TEntity> All { get; }
		int Count(params Expression<Func<TEntity, bool>>[] where);
		int Count();
		TEntity Find(params object[] keyValues);
		IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
		IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
		IEnumerable<TEntity> Where(params Expression<Func<TEntity, bool>>[] conditions);
		IEnumerable<TEntity> Where<TOrderByType>(ref OrderedPagingOptions<TEntity, TOrderByType> pagingOptions, params Expression<Func<TEntity, bool>>[] conditions);
		IEnumerable<TEntity> ToPagedList(ref PagingOptions paging);
		IEnumerable<TEntity> ToPagedList<TOrderByType>(ref OrderedPagingOptions<TEntity, TOrderByType> paging);
		TEntity First(Expression<Func<TEntity, bool>> condition);
		void Add(TEntity entity);
		void Delete(TEntity entity);

		/// <summary>
		/// Tries to find an Entity with the passed keyValues and marks it to be deleted when Save() is called
		/// </summary>
		/// <param name="keyValues">The values of the primary keys.</param>
		/// <returns>True if the entity is found, otherwise, false.</returns>
        bool Delete(params object[] keyValues);
        void MarkAsModified(TEntity entity);
	    void Attach(TEntity entity);

		/// <summary>
		/// Saves all changes made in this context to the underlying database.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">Thrown if the context has been disposed.</exception>
		/// <exception cref="DbEntityValidationException">Thrown if any of the models being saved isn't valid.</exception>
		/// <returns>The number of objects written to the underlying database.</returns>
		int Save();

	}
}