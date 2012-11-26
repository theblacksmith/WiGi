namespace WiGi.UI.Models
{
	using DotNetOpenAuth.AspNet;

	public class ProviderFilter
	{
		public static bool IsAllowed(AuthenticationResult result)
		{
			return result.Provider == "google" && result.UserName.EndsWith("camiseteria.com");
		}
	}
}