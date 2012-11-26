namespace WiGi
{
	using Git;
	using NGit.Api;

	public class WiGiManager
	{
		public static PullResult UpdateWikiRepository()
		{
			return GitHelper.UpdateRepository(
				WG.Settings.RepositoryGitPath,
				WG.Settings.GitUser,
				WG.Settings.GitPassword
			);
		}
	}
}
