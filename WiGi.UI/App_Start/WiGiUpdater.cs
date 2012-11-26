[assembly: WebActivator.PostApplicationStartMethod(typeof(WiGi.UI.AttributeRoutingConfig), "Start")]

namespace WiGi.UI
{
	using System.Web;
	using Git;

	public class WiGiUpdater
	{
		public static void Start()
		{
			WiGiManager.UpdateWikiRepository();
		}
	}
}