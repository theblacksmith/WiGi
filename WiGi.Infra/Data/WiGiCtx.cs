namespace WiGi.Data
{
	using System.Data.Entity;
	using Account.Mapping;
	using WiGi.Account;
	using WiGi.Wiki;
	using Wiki.Mapping;

	public class WiGiCtx : BaseDbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Page> Pages { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Category> Categories { get; set; }
		
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations
				.Add(new TagMapping())
				.Add(new PageMapping())
				.Add(new CategoryMapping())
				.Add(new UserMapping());
		}
	}
}
