namespace WiGi.Data
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data.Common;
	using System.Data.Entity;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.Validation;
	using System.Linq.Expressions;

    public class BaseDbContext : DbContext
    {
    	
    	public BaseDbContext() 
			: base("name=DefaultConnection")
        {
        }
    	
    	public BaseDbContext(string nameOrConnectionString) 
			: base(nameOrConnectionString)
    	{	
    	}
    
    	public BaseDbContext(string nameOrConnectionString, DbCompiledModel model) 
			: base(nameOrConnectionString, model)
    	{
    	}
    
    	public BaseDbContext(DbConnection existingConnection, bool contextOwnsConnection) 
			: base(existingConnection, contextOwnsConnection)
    	{
    	}

		public BaseDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
			: base(existingConnection, model, contextOwnsConnection)
    	{
    	}

        public DbEntityValidationResult ValidateProperties<TEntity>(DbEntityEntry<TEntity> entry, params Expression<Func<TEntity, object>>[] properties) where TEntity : class
        {
            ICollection<DbValidationError> errors = new Collection<DbValidationError>();
            
            foreach (var prop in properties) if (entry.Property(prop).IsModified)
                {
                    foreach (var e in entry.Property(prop).GetValidationErrors())
                        errors.Add(e);
                }

            return new DbEntityValidationResult(entry, errors);
        }


		public void Import<TContext>(DbModelBuilder modelBuilder) where TContext : BaseDbContext, new()
		{
			(new TContext()).OnModelCreating(modelBuilder);
		}
    }
}
