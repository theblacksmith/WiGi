using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WiGi.Tests
{
	using System.Diagnostics;
	using System.IO;
	using Git;
	using Git.Commands;

	[TestClass]
	public class GitTests
	{
		private string _resourcesDir;
		private string ResourcesDir
		{
			get
			{
				if(_resourcesDir == null)
					_resourcesDir = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)), "Resources");

				return _resourcesDir;
			}
		}
		[TestMethod]
		public void CanClone()
		{
			var git = new Git(ResourcesDir);

			var cmd = new CloneCommand(Path.Combine(ResourcesDir, "RemoteRepo"));
			cmd.Local = true;
			cmd.Directory = "Clone";
			var repo = git.Clone(cmd);

			Debug.Write(git.Output);

			Assert.IsNotNull(repo);
			Assert.IsTrue(repo.IsGitRepo);
		}

		[TestMethod]
		public void CanParseConfig()
		{
			var conf = RepoConfig.Parse(Path.Combine(ResourcesDir,"sample-config"));

			Assert.IsNotNull(conf);
			Assert.IsFalse(conf.Bare);
		}
	}
}
