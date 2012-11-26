namespace WiGi.Data.Account.Mapping
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.ModelConfiguration;
	using WiGi.Account;

	class UserMapping : EntityTypeConfiguration<User>
	{
		public UserMapping()
		{
			HasKey(m => m.Id);
			Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(m => m.IsAdmin).IsRequired();
		}
	}
}
