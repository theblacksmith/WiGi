namespace WiGi
{
	using System.IO;
	using System.Web;
	using Ninject;
	using Services;

	public class WiGiSettings
	{
		private ISettingsProvider _settings;

		public WiGiSettings(ISettingsProvider settingsProvider)
		{
			_settings = settingsProvider;
			Auth = new WiGiAuthSettings(settingsProvider);
		}

		public string Repository
		{
			get { return _settings.GetSetting<string>("Repository"); }
			set { _settings.SetSetting("Repository", value); }
		}

		public string GitUser
		{
			get { return _settings.GetSetting<string>("GitUser"); }
			set { _settings.SetSetting("GitUser", value); }
		}

		public string GitPassword
		{
			get { return _settings.GetSetting<string>("GitPassword"); }
			set { _settings.SetSetting("GitPassword", value); }
		}

		public bool AllowRegistration
		{
			get { return _settings.GetSetting<bool>("AllowRegistration"); }
			set { _settings.SetSetting("AllowRegistration", value); }
		}

		public bool UseSameGitAccountForAllUsers
		{
			get { return _settings.GetSetting<bool>("UseSameGitAccountForAllUsers"); }
			set { _settings.SetSetting("UseSameGitAccountForAllUsers", value); }
		}

		public string DatabaseProvider
		{
			get { return _settings.GetSetting<string>("DatabaseProvider"); }
			set { _settings.SetSetting("DatabaseProvider", value); }
		}

		public string ConnectionString
		{
			get { return _settings.GetSetting<string>("ConnectionString"); }
			set { _settings.SetSetting("ConnectionString", value); }
		}

		//
		//  ADMINISTRATION
		//

		public string AdminUsername
		{
			get { return _settings.GetSetting<string>("AdminUsername"); }
			set { _settings.SetSetting("AdminUsername", value); }
		}

		/// <summary>
		/// The path of the repository relative to the application root
		/// </summary>
		public string RepositoryPath
		{
			get { return "~/App_Data/wikirepo"; }
		}

		/// <summary>
		/// The real (physical) path of the repository
		/// </summary>
		public string RepositoryGitPath
		{
			get { return Path.Combine(HttpContext.Current.Server.MapPath(RepositoryPath), ".git"); }
		}

		public WiGiAuthSettings Auth { get; set; }

		public void Save()
		{
			_settings.Save();
		}
	}

	public class WiGiAuthSettings
	{
		private readonly ISettingsProvider _settings;

		public WiGiAuthSettings(ISettingsProvider settingsProvider)
		{
			_settings = settingsProvider;
		}

		public bool EnableGoogleLogin
		{
			get { return _settings.GetSetting<bool>("Auth.EnableGoogleLogin"); }
			set { _settings.SetSetting("Auth.EnableGoogleLogin", value); }
		}

		public string GoogleAppsDomain
		{
			get { return _settings.GetSetting<string>("Auth.GoogleAppsDomain"); }
			set { _settings.SetSetting("Auth.GoogleAppsDomain", value); }
		}
	}
}
