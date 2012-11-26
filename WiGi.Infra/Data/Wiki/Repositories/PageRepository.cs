namespace WiGi.Data.Wiki.Repositories
{
	using WiGi.Wiki;
	using WiGi.Wiki.Repositories;

	public class PageRepository : Repository<WiGiCtx, Page>, IPageRepository
	{
		public PageRepository(WiGiCtx ctx) : base(ctx)
		{
		}

		public Page Find(string docId)
		{
			return First(p => p.DocId == docId);
		}
	}
}
