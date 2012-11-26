namespace WiGi.Wiki.Repositories
{
	public interface IPageRepository : IRepository<Page>
	{
		Page Find(string docId);
	}
}
