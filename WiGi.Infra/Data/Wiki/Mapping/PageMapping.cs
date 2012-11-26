namespace WiGi.Data.Wiki.Mapping
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.ModelConfiguration;
	using WiGi.Wiki;

	class PageMapping : EntityTypeConfiguration<Page>
	{
		public PageMapping()
		{
			HasKey(m => m.Id);
			Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Ignore(m => m.Content);
		}
	}
}