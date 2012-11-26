namespace WiGi.Services
{
	using System.Collections.Generic;

	public interface ISettingsProvider
	{
		IDictionary<string, string> GetAllSettings();
		T GetSetting<T>(string key);
		void SetSetting<T>(string key, T value);
		void Save();
	}
}