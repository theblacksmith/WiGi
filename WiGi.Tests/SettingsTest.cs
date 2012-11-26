using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WiGi.Tests
{
	using System.Configuration;
	using System.Web.Configuration;

	[TestClass]
	public class SettingsTest
	{
		[TestMethod]
		public void WebConfig()
		{
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			config.AppSettings.Settings["SiteName"].Value = "New Site Name Value";
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");  
		}
	}
}
