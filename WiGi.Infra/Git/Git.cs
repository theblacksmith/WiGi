namespace WiGi.Git
{
	using System.IO;
	using Commands;
	using WebGitNet;

	public class Git
	{
		public string WorkingDir { get; set; }

		/// <summary>
		/// Stores the output of the last command
		/// </summary>
		public string Output { get; set; }

		public Git(string workingDir)
		{
			WorkingDir = workingDir;
		}

		public Repository Clone(string url, string dir = "", bool bare = false)
		{
			var cloneCmd = new CloneCommand(url, dir);
			cloneCmd.Bare = bare;

			return Clone(cloneCmd);
		}

		public Repository Clone(CloneCommand cmd)
		{
			Output = GitUtilities.Execute(cmd.ToString(), WorkingDir);
			
			return Repository.ForDir(Path.Combine(WorkingDir, cmd.Directory));
		}
	}
}
