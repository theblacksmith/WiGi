namespace WiGi.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data.Entity.Validation;
    using Domain;

	public class Scope<TEntity>
		where TEntity : class
	{
		public readonly string Name;
		public readonly Expression<Func<TEntity, bool>> Condition;
		public readonly bool IsPermanent;

		public Scope(string name, Expression<Func<TEntity, bool>> condition, bool isPermanent)
		{
			Name = name;
			Condition = condition;
			IsPermanent = isPermanent;
		}
	}

	public abstract class Repository<TContext, TEntity> : IRepository<TEntity>
		where TEntity : class
		where TContext : BaseDbContext, new()
    {
		protected Dictionary<String, Scope<TEntity>> NamedScopes = new Dictionary<string, Scope<TEntity>>();
		
		public void AddScope(string name, Expression<Func<TEntity, bool>> condition)
		{
			NamedScopes.Add(name, new Scope<TEntity>(name, condition, false));
		}

		public void AddScope(string name, Expression<Func<TEntity, bool>> condition, bool permanent)
		{
			NamedScopes.Add(name, new Scope<TEntity>(name, condition, permanent));
		}

		public bool RemoveScope(string name)
		{
			return NamedScopes.Remove(name);
		}

		protected void RemoveVolatileScopes()
		{
			var toRemove = (from s in NamedScopes where s.Value.IsPermanent == false select s.Key).ToList();
			foreach (var key in toRemove)
				NamedScopes.Remove(key);
		}

        /// <summary>
        /// Store the last Save call DbEntityValidationResult's
        /// </summary>
        protected IEnumerable<IValidationResult> EntityValidationErrors { get; set; }

        private TContext _context;
		protected TContext Context
		{
			get { return _context; }
		    set { _context = value; }
		}

		public Repository(TContext ctx)
		{
			Context = ctx;
		}

		public virtual IEnumerable<TEntity> All
		{
			get
			{
				return GetAll();
			}
		}

		public virtual IEnumerable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = _context.Set<TEntity>();
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
			return query.AsEnumerable();
		}

		/// <summary>
		/// Returns an IQueryable to be used internally ONLY. The result set is already scoped.
		/// </summary>
		/// <returns>Scoped queryable list of entities</returns>
		protected virtual IQueryable<TEntity> GetAll()
		{
			var query = _context.Set<TEntity>().AsQueryable();

			query = NamedScopes.Values.Aggregate(query, (current, scope) => current.Where(scope.Condition));

			RemoveVolatileScopes();

			return query;
		}

		public virtual TEntity Find(params object[] keyValues)
		{
			return _context.Set<TEntity>().Find(keyValues);
		}

		public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
		{
			IEnumerable<TEntity> query = GetAll().Where(predicate);
			return query;
		}

		public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = GetAll().Where(predicate);

			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}

			return query.AsEnumerable();
		}

		public virtual IEnumerable<TEntity> Where(params Expression<Func<TEntity, bool>>[] conditions)
		{
			var query = GetAll();

			query = conditions.Aggregate(query, (current, condition) => current.Where(condition));

			return query;
		}

		public virtual int Count(params Expression<Func<TEntity, bool>>[] where)
		{
			var query = GetAll();

			query = where.Aggregate(query, (current, condition) => current.Where(condition));

			return query.Count();
		}

		public virtual int Count()
		{
			return GetAll().Count();
		}

		public TEntity First(Expression<Func<TEntity, bool>> condition)
		{
			return GetAll().FirstOrDefault(condition);
		}

		public virtual IEnumerable<TEntity> Where<TOrderByType>(ref OrderedPagingOptions<TEntity, TOrderByType> paging, params Expression<Func<TEntity, bool>>[] conditions)
		{
			var query = GetAll();

			query = conditions.Aggregate(query, (current, condition) => current.Where(condition));

			paging.Total = query.Count();

			query = query.OrderBy(paging.OrderBy);
			query = query.Skip((paging.Page - 1) * paging.PageSize);
			query = query.Take(paging.PageSize);

			return query.AsEnumerable();
		}

		public virtual IEnumerable<TEntity> ToPagedList(ref PagingOptions paging)
		{
			var query = GetAll();

			paging.Total = query.Count();

			query = query.Take(paging.PageSize);

			return query.AsEnumerable();
		}

		public virtual IEnumerable<TEntity> ToPagedList<TOrderByType>(ref OrderedPagingOptions<TEntity, TOrderByType> paging)
		{
			var query = GetAll();

			paging.Total = query.Count();

			query = query.OrderBy(paging.OrderBy);
			query = query.Skip((paging.Page - 1) * paging.PageSize);
			query = query.Take(paging.PageSize);

			return query.AsEnumerable();
		}

		public virtual void Add(TEntity entity)
		{
			_context.Set<TEntity>().Add(entity);
		}

		public virtual void Delete(TEntity entity)
		{
			_context.Set<TEntity>().Remove(entity);
		}

		/// <summary>
		/// Tries to find an Entity with the passed keyValues and marks it to be deleted when Save() is called
		/// </summary>
		/// <param name="keyValues">The values of the primary keys.</param>
		/// <returns>True if the entity is found, otherwise, false.</returns>
		public bool Delete(params object[] keyValues)
		{
			var entity = Find(keyValues);

			if (entity != null)
			{
				Delete(entity);
				return true;
			}

			return false;
		}

        public bool IsValid(TEntity entity)
        {
            return _context.Entry(entity).GetValidationResult().IsValid;
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        /// <exception cref="DbEntityValidationException">Thrown if any of the models being saved isn't valid.</exception>
        /// <returns>The number of objects written to the underlying database.</returns>
		public virtual int Save()
        {
            int writtenCount;
            try
            {
                writtenCount = _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                EntityValidationErrors = dbEx.EntityValidationErrors.ToEFValidationResults();
                var msg = "";
                foreach (var entityErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in entityErrors.ValidationErrors)
                    {
                        msg += String.Format("Entity: {0} Property: {1} Error: {2}", entityErrors.Entry.GetType().Name, validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                throw new Exception(msg);
            }

            return writtenCount;
		}


        public virtual void MarkAsModified(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Attach(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
        }

		#region IDisposable Impl
		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
				if (disposing) {/*
					will be disposed by NInject
					_context.Dispose();
				*/}

			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}