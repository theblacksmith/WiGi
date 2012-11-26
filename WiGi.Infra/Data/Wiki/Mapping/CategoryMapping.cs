namespace WiGi.Data.Wiki.Mapping
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.ModelConfiguration;
	using WiGi.Wiki;

	class CategoryMapping : EntityTypeConfiguration<Category>
	{
		public CategoryMapping()
		{
			HasKey(m => m.Id);
			Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}
	}
}