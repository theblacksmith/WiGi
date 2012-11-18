namespace WiGi.Git
{
	using Commands;
	using WebGitNet;

	public class Repository
	{
		private RepoInfo _repoInfo;

		public string Name
		{
			get { return _repoInfo.Name; }
			set { _repoInfo.Name = value; }
		}

		public string Description
		{
			get { return _repoInfo.Description; }
			set { _repoInfo.Description = value; }
		}

		public bool IsArchived
		{
			get { return _repoInfo.IsArchived; }
			set { _repoInfo.IsArchived = value; }
		}

		public bool IsGitRepo
		{
			get { return _repoInfo.IsGitRepo; }
			set { _repoInfo.IsGitRepo = value; }
		}

		public string Path { get; set; }

		private Repository()
		{
			
		}

		public static Repository ForDir(string dir)
		{
			var info = GitUtilities.GetRepoInfo(dir);

			if (!info.IsGitRepo)
				return null;

			var repo = new Repository();
			repo._repoInfo = info;
			repo.Path = dir;

			return repo;
		}

		public string Pull()
		{
			return GitUtilities.Execute("pull", Path);
		}

		public string Pull(PullCommand cmd)
		{
			return GitUtilities.Execute(cmd.ToString(), Path);
		}

		public string Push()
		{
			return GitUtilities.Execute("push", Path);
		}

		public string Pull(PushCommand cmd)
		{
			return GitUtilities.Execute(cmd.ToString(), Path);
		}
	}
}
