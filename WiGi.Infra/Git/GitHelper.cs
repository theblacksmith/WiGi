namespace WiGi.Git
{
	using System.IO;
	using NGit.Api;
	using NGit.Storage.File;
	using NGit.Transport;
	using System.Web.WebPages;

	public static class GitHelper
	{	
		/// <summary>
		/// Code extracted from http://stackoverflow.com/questions/4345340/equivalent-of-java-net-urlconnection-in-net
		/// </summary>
		/// <param name="repoPath"></param>
		/// <returns></returns>
		public static bool isValidLocalRepository(string repoPath)
		{
			bool result;
			try
			{
				result = new FileRepository(repoPath).ObjectDatabase.Exists();
			}
			catch (IOException)
			{
				result = false;
			}
			return result;
		}

		public static PullResult UpdateRepository(string repoPath, string user, string password)
		{
			var repo = new FileRepository(repoPath);
			var g = new Git(repo);

			var cmd = g.Pull();

			if(!user.IsEmpty())
				cmd.SetCredentialsProvider(new UsernamePasswordCredentialsProvider(user, password));

			return cmd.Call();
		}
	}
}
