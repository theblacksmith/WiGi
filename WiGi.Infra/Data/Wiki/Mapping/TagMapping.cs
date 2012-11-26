namespace WiGi.Data.Wiki.Mapping
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.ModelConfiguration;
	using WiGi.Wiki;

	class TagMapping : EntityTypeConfiguration<Tag>
	{
		public TagMapping()
		{
			HasKey(m => m.Id);
			Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}
	}
}