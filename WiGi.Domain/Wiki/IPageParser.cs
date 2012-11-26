namespace WiGi.Wiki
{
	public interface IPageParser
	{
		bool SupportsMetaInfo { get; }
		string Parse(Page page);
		dynamic GetMetaInfo(Page page);
	}
}