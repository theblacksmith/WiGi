namespace WiGi
{
	using System;
	using System.Data.Common;
	using System.IO;
	using Account.Repositories;
	using Git;
	using NGit;
	using NGit.Api;
	using NGit.Storage.File;
	using NGit.Transport;
	using Ninject;

	public enum InstallerMessage
	{
		ExceptionThrown,
		UnkownFailure,
		Success,
		UserDoesNotExists,
		InvalidConnectionString,
		InvalidDbProvider,
		CouldNotConnectToDb
	}

	public class Installer
	{
		private string _appRoot;

		private string _repoPath;

		public string GitOutput { get; private set; }

		public Exception LastException { get; private set; }

		[Inject]
		public IUserRepository _userRepo;

		public Installer(string appRoot)
		{
			_appRoot = appRoot;
			_repoPath = Path.Combine(_appRoot, @"App_Data\wikirepo");
		}

		public bool IsInstalled()
		{
			return RepoIsClonned() && AdminAccountExists();
		}

		private bool RepoIsClonned()
		{
			return GitHelper.isValidLocalRepository(Path.Combine(_repoPath, @".git"));
		}

		private bool AdminAccountExists()
		{
			return !String.IsNullOrEmpty(WG.Settings.AdminUsername) && _userRepo.FindByUsername(WG.Settings.AdminUsername) != null;
		}

		public InstallerMessage SetRepository(string uri, string user, string password, bool replaceExisting = false)
		{
			LastException = null;

			try
			{
				if (RepoIsClonned())
				{
					if (replaceExisting)
					{
						try
						{
							Directory.Delete(_repoPath, true);
						}
						catch (Exception e)
						{
							throw new Exception("Could not delete current repository. " + e.Message);
						}
						
					}
					else
					{
						if (!GitHelper.isValidLocalRepository(Path.Combine(_repoPath, @".git")))
							throw new Exception("There are files stored where the repository would be cloned. Clone canceled");

						var repo = new FileRepository(Path.Combine(_repoPath, @".git"));

						var c = repo.GetConfig();
						var remoteUrl = c.GetString("remote", "origin", "url");
						var isSameRepo = remoteUrl != uri;

						if (!isSameRepo)
						{
							var remotes = c.GetSubsections("remote");

							foreach (var remote in remotes)
							{
								var rUrl = c.GetString("remote", remote, "url");

								if (String.IsNullOrEmpty(remoteUrl)) // let's keep the first we find
									remoteUrl = rUrl;

								if (rUrl == uri)
								{
									isSameRepo = true;
									break;
								}
							}

							if (!isSameRepo)
							{
								if (!String.IsNullOrEmpty(remoteUrl))
									throw new Exception("There is already a repository pointing to " + remoteUrl + " where the wiki should be cloned.");

								throw new Exception("There is already a repository where the wiki should be cloned.");
							}
						}

						return InstallerMessage.Success;
					}
				}

				if (!Directory.Exists(_repoPath))
				{
					if (!Directory.Exists(Path.Combine(_appRoot, "App_Data")))
						Directory.CreateDirectory(Path.Combine(_appRoot, "App_Data"));

					Directory.CreateDirectory(_repoPath);
				}
				var cmd = new CloneCommand();

				if(!String.IsNullOrEmpty(user))
					cmd.SetCredentialsProvider(new UsernamePasswordCredentialsProvider(user, password));

				cmd.SetURI(uri);
				cmd.SetCloneSubmodules(true);
				cmd.SetDirectory(_repoPath);

				cmd.Call();
			}
			catch (Exception e)
			{
				LastException = e;
				return InstallerMessage.ExceptionThrown;
			}
			
			if (RepoIsClonned())
			{
				WG.Settings.Repository = uri;
				return InstallerMessage.Success;
			}

			return InstallerMessage.UnkownFailure;
		}

		public InstallerMessage SetAdmin(string username)
		{
			var newAdmin = _userRepo.FindByUsername(username);

			if (newAdmin == null)
				return InstallerMessage.UserDoesNotExists;

			if (!String.IsNullOrEmpty(WG.Settings.AdminUsername)) {
				var admin = _userRepo.FindByUsername(WG.Settings.AdminUsername);

				if (admin != null)
					admin.IsAdmin = false;
			}

			WG.Settings.AdminUsername = newAdmin.Username;
			newAdmin.IsAdmin = true;

			_userRepo.Save();

			return InstallerMessage.Success;
		}

		public InstallerMessage SetDbConnection(string connectionString, string provider)
		{
			// validates string
			DbConnectionStringBuilder csb = new DbConnectionStringBuilder();
			try
			{
				csb.ConnectionString = connectionString;
			}
			catch (Exception e)
			{
				LastException = e;
				return InstallerMessage.InvalidConnectionString;
			}

			// test database connection
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
				using (DbConnection conn = factory.CreateConnection())
				{
					if (conn == null)
						return InstallerMessage.InvalidDbProvider;

					conn.ConnectionString = connectionString;
					conn.Open();
				}
			}
			catch (Exception e)
			{
				LastException = e;
				return InstallerMessage.CouldNotConnectToDb;
			}

			WG.Settings.ConnectionString = connectionString;
			WG.Settings.DatabaseProvider = provider;

			return InstallerMessage.Success;
		}
	}
}
