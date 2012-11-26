namespace WiGi.Services
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Web.Configuration;

	public class ConfigFileSettingsProvider : ISettingsProvider
	{
		public IDictionary<string, string> GetAllSettings()
		{
			var settings = new Dictionary<string, string>();
			var appSettings = WebConfigurationManager.AppSettings;
			foreach (var setting in appSettings.AllKeys)
			{
				settings.Add(setting, appSettings[setting]);
			}

			return settings;
		}

		public T GetSetting<T>(string key)
		{
			var result = default(T);

			if (key == "ConnectionString")
			{
				return CommonUtils.To<T>(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
			}

			if (key == "DatabaseProvider")
			{
				return CommonUtils.To<T>(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ProviderName);
			}

			var setting = WebConfigurationManager.AppSettings[key];
			if (!string.IsNullOrEmpty(setting))
				result = CommonUtils.To<T>(setting);
			return result;
		}

		public void SetSetting<T>(string key, T value)
		{
			var val = "";
			
			if (key == "ConnectionString")
			{
				SetConnectionString(value.ToString());
				return;
			}

			if (key == "DatabaseProvider")
			{
				SetProvider(value.ToString());
				return;
			}

			if (value is bool)
				val = Convert.ToBoolean(value) ? "true" : "false";
			else if (value != null)
				val = value.ToString();
			else
				val = "";

			WebConfigurationManager.AppSettings[key] = val;
		}

		private void SetProvider(string provider)
		{
			//WebConfigurationManager.ConnectionStrings["DefaultConnection"].ProviderName = provider;
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			ConnectionStringsSection section = config.GetSection("connectionStrings") as ConnectionStringsSection;
			if (section != null)
			{
				section.ConnectionStrings["DefaultConnection"].ProviderName = provider;
				config.Save();
			}
		}

		private void SetConnectionString(string connectionString)
		{
			//WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString = connectionString;
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			ConnectionStringsSection section = config.GetSection("connectionStrings") as ConnectionStringsSection;
			if (section != null)
			{
				section.ConnectionStrings["DefaultConnection"].ConnectionString = connectionString;
				config.Save();
			}
		}

		public void Save()
		{
			var setts = GetAllSettings();

			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

			foreach (var sett in setts)
			{
				if (config.AppSettings.Settings[sett.Key] == null)
					config.AppSettings.Settings.Add(sett.Key, sett.Value);
				else
					config.AppSettings.Settings[sett.Key].Value = sett.Value;
			}

			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}
	}
}
