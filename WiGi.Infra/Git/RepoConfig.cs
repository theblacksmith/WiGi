namespace WiGi.Git
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text.RegularExpressions;

	public class RemoteConfig
	{
		public string Name { get; set; }
		public string Fetch { get; set; }
		public string Url { get; set; }
		public Dictionary<string, string> Options { get; set; }

		public RemoteConfig(string name)
		{
			Name = name;
		}
	}

	public class BranchConfig
	{
		public string Name { get; set; }
		public string Remote { get; set; }
		public string Merge { get; set; }
		public Dictionary<string, string> Options { get; set; }

		public BranchConfig(string name)
		{
			Name = name;
		}
	}

	public class RepoConfig
	{
		private static string OPTION_LINE_REGEX = @"\s*([\w]+)\s*=\s*(.*)";

		/// <summary>
		/// If true this repository is assumed to be bare and has no working directory associated with it. If this is the case a number of commands that require 
		/// a working directory will be disabled, such as git-add(1) or git-merge(1). 
		/// This setting is automatically guessed by git-clone(1) or git-init(1) when the repository was created. By default a repository that ends in "/.git" is 
		/// assumed to be not bare (bare = false), while all other repositories are assumed to be bare (bare = true).
		/// </summary>
		public bool Bare { get; set; }

		/// <summary>
		/// If false, symbolic links are checked out as small plain files that contain the link text. git-update-index(1) and git-add(1) 
		/// will not change the recorded type to regular file. Useful on filesystems like FAT that do not support symbolic links.
		/// The default is true, except git-clone(1) or git-init(1) will probe and set core.symlinks false if appropriate when the repository is created.
		/// </summary>
		public bool Symlinks { get; set; }

		/// <summary>
		/// If true, this option enables various workarounds to enable git to work better on filesystems that are not case sensitive, like FAT. 
		/// For example, if a directory listing finds "makefile" when git expects "Makefile", git will assume it is really the same file, and 
		/// continue to remember it as "Makefile".
		/// The default is false, except git-clone(1) or git-init(1) will probe and set core.ignorecase true if appropriate when the repository is created.
		/// </summary>
		public bool IgnoreCase { get; set; }

		/// <summary>
		/// Setting this variable to "true" is almost the same as setting the text attribute to "auto" on all files except that 
		/// text files are not guaranteed to be normalized: files that contain CRLF in the repository will not be touched. Use 
		/// this setting if you want to have CRLF line endings in your working directory even though the repository does not have 
		/// normalized line endings. This variable can be set to input, in which case no output conversion is performed.
		/// </summary>
		public bool AutoCRLF { get; set; }

		/// <summary>
		/// If false, the executable bit differences between the index and the working tree are ignored; useful on broken filesystems like FAT. See git-update-index(1).
		/// The default is true, except git-clone(1) or git-init(1) will probe and set core.fileMode false if appropriate when the repository is created.
		/// </summary>
		public bool FileMode { get; set; }

		public Dictionary<string, RemoteConfig> Remotes;

		public Dictionary<string, BranchConfig> Branches;

		public RepoConfig()
		{
			Remotes = new Dictionary<string, RemoteConfig>();
			Branches = new Dictionary<string, BranchConfig>();
		}

		/// <summary>
		/// Parses a .git config file and returns the next line or null
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static RepoConfig Parse(string filePath)
		{
			var repo = new RepoConfig();
			var stream = new StreamReader(filePath);

			var line = stream.ReadLine();

			while (!stream.EndOfStream)
			{
				if(line == null)
					continue;

				line = line.Trim();

				if (IsSectionLine(line) && !stream.EndOfStream)
				{
					if (line.StartsWith("[core"))
						line = ParseCore(line, stream, repo);

					if(line == null) continue;

					if (line.StartsWith("[remote"))
						line = ParseRemote(line, stream, repo);

					if (line == null) continue;

					if (line.StartsWith("[branch"))
						line = ParseBranch(line, stream, repo);
				}
			}

			return repo;
		}

		/// <summary>
		/// Parses a core section and returns the next line or null
		/// </summary>
		/// <param name="line"></param>
		/// <param name="stream"></param>
		/// <param name="repo"></param>
		/// <returns></returns>
		private static string ParseCore(string line, StreamReader stream, RepoConfig repo)
		{
			var opts = new Dictionary<string, string>();

			line = stream.ReadLine();

			while (!IsSectionLine(line) && !stream.EndOfStream)
			{
				var splitted = line.ToLower().Split(new[] { '=' }, 2);

				switch (splitted[0].Trim())
				{
					case "autocrlf":
						repo.AutoCRLF = (splitted[1] == "true");
						break;
					case "bare":
						repo.Bare = (splitted[1] == "true");
						break;
					case "filemode":
						repo.FileMode = (splitted[1] == "true");
						break;
					case "ignorecase":
						repo.IgnoreCase = (splitted[1] == "true");
						break;
					case "symlinks":
						repo.Symlinks = (splitted[1] == "true");
						break;
				}
				opts.Add(splitted[0].Trim(), splitted[1].Trim());
				line = stream.ReadLine();
			}

			return line;
		}

		private static string ParseBranch(string line, StreamReader stream, RepoConfig repo)
		{
			var name = Regex.Match(line, "\\[branch\\s*\"(.*)\"\\s*\\]").Groups[1].Value;
			var options = new Dictionary<string, string>();

			line = stream.ReadLine();
			while (!IsSectionLine(line) && !stream.EndOfStream)
			{
				var match = new Regex(OPTION_LINE_REGEX).Match(line);
				options.Add(match.Groups[1].Value, match.Groups[2].Value);
				line = stream.ReadLine();
			}

			repo.AddBranch(name, options);
			return line;
		}

		public void AddBranch(string name, Dictionary<string, string> options)
		{
			var conf = new BranchConfig(name);

			if (options.ContainsKey("remote"))
				conf.Remote = options["remote"];

			if (options.ContainsKey("merge"))
				conf.Merge = options["merge"];

			conf.Options = options;
			Branches[name] = conf;
		}

		/// <summary>
		/// Parses a remote section and returns the next line or null
		/// </summary>
		/// <param name="line"></param>
		/// <param name="stream"></param>
		/// <param name="repo"></param>
		/// <returns></returns>
		private static string ParseRemote(string line, StreamReader stream, RepoConfig repo)
		{
			var remote = Regex.Match(line, "\\[remote\\s*\"(.*)\"\\s*\\]").Groups[1].Value;
			var opts = new Dictionary<string, string>();

			line = stream.ReadLine();
			while (!IsSectionLine(line) && !stream.EndOfStream)
			{
				var match = new Regex(OPTION_LINE_REGEX).Match(line);
				opts.Add(match.Groups[1].Value, match.Groups[2].Value);
				line = stream.ReadLine();
			}

			repo.AddRemote(remote, opts);
			return line;
		}

		public void AddRemote(string name, Dictionary<string,string> options)
		{
			var conf = new RemoteConfig(name);

			if (options.ContainsKey("fetch"))
				conf.Fetch = options["fetch"];

			if (options.ContainsKey("url"))
				conf.Url = options["url"];

			conf.Options = options;
			Remotes[name] = conf;
		}

		private static bool IsSectionLine(string line)
		{
			return line != null && line.Trim().StartsWith("[");
		}
	}
}
