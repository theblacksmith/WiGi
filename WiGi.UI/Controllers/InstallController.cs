namespace WiGi.UI.Controllers
{
	using System.Configuration;
	using System.Web.Configuration;
	using System.Web.Mvc;
	using Account.Repositories;
	using AttributeRouting;
	using AttributeRouting.Web.Mvc;
	using Filters;
	using Models;
	using WebMatrix.WebData;

	[InitializeSimpleMembership]
	[RoutePrefix("Install")]
	public class InstallController : BaseController
	{
		private IUserRepository _usrRepo;

		public InstallController(IUserRepository usrRepo)
		{
			_usrRepo = usrRepo;
		}

		[AllowAnonymous]
		[GET("")]
		public ActionResult Index()
		{
				var m = new InstallModel()
				{
					AdminUsername = WG.Settings.AdminUsername,
					AllowRegistration = WG.Settings.AllowRegistration,
					ConnectionString = WG.Settings.ConnectionString,
					DatabaseProvider = WG.Settings.DatabaseProvider,
					EnableGoogleLogin = WG.Settings.Auth.EnableGoogleLogin,
					GitUser = WG.Settings.GitUser,
					GitPassword = WG.Settings.GitPassword,
					GoogleAppsDomain = WG.Settings.Auth.GoogleAppsDomain,
					UseSameGitAccountForAllUsers = WG.Settings.UseSameGitAccountForAllUsers,
					WikiRepository = WG.Settings.Repository
				};
			return View(m);
		}

		[AllowAnonymous]
		[POST("")]
		public ActionResult Index(InstallModel model)
		{
			var installer = new Installer(Server.MapPath("~"));

			if (ModelState.IsValid)
			{
				var ret = installer.SetRepository(model.WikiRepository, model.GitUser, model.GitPassword, false);
				var allIsWell = true;

				switch(ret)
				{
					case InstallerMessage.ExceptionThrown :
						ModelState.AddModelError("WikiRepository", installer.LastException.Message);
						allIsWell = false;
						break;
				
					case InstallerMessage.UnkownFailure :
						ModelState.AddModelError("WikiRepository", "We couldn't clone the wiki repository. Unfortunately, we don't know why :(");
						allIsWell = false;
						break;
				}

				var conf = WebConfigurationManager.OpenWebConfiguration("~");

				ret = installer.SetDbConnection(model.ConnectionString, model.DatabaseProvider);

				switch (ret)
				{
					case InstallerMessage.CouldNotConnectToDb :
						ModelState.AddModelError("ConnectionString", "Could not connect to the database.");
						allIsWell = false;
						break;

					case InstallerMessage.InvalidConnectionString :
						ModelState.AddModelError("ConnectionString", "The connection string is invalid.");
						allIsWell = false;
						break;

					case InstallerMessage.InvalidDbProvider:
						ModelState.AddModelError("DatabaseProvider", "This provider could not be found.");
						allIsWell = false;
						break;
				}

				if (!allIsWell)
					return View();

				WG.Settings.Repository = model.WikiRepository;
				WG.Settings.GitUser = model.GitUser;
				WG.Settings.GitPassword = model.GitPassword;
				WG.Settings.AdminUsername = model.AdminUsername;
				WG.Settings.AllowRegistration = model.AllowRegistration;
				WG.Settings.UseSameGitAccountForAllUsers = model.UseSameGitAccountForAllUsers;
				WG.Settings.Auth.EnableGoogleLogin = model.EnableGoogleLogin;
				WG.Settings.Auth.GoogleAppsDomain = model.GoogleAppsDomain;
				
				WG.Settings.Save();

				conf.Save(ConfigurationSaveMode.Full);

				// creating or update admin user
				var admin = _usrRepo.FindByUsername(model.AdminUsername);

				if (WebSecurity.UserExists(model.AdminUsername))
					WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(model.AdminUsername), model.AdminPassword);
				else
					WebSecurity.CreateUserAndAccount(model.AdminUsername, model.AdminPassword);

				WebSecurity.Login(model.AdminUsername, model.AdminPassword);

				installer.SetAdmin(model.AdminUsername);

				return RedirectToAction("Index", "Home");
			}

			return View();
		}

    }
}
