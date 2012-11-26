using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WiGi.Tests
{
	using System.IO;

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
			if(Directory.Exists(Path.Combine(ResourcesDir, "Clone")))
				Directory.Delete(Path.Combine(ResourcesDir, "Clone"),true);

			var cmd = new NGit.Api.CloneCommand();
			cmd.SetURI(Path.Combine(ResourcesDir, "RemoteRepo"));
			cmd.SetDirectory(Path.Combine(ResourcesDir, "Clone"));
			
			var git = cmd.Call();

			Assert.IsNotNull(git.GetRepository());
		}
	}
}
