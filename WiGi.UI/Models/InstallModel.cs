namespace WiGi.UI.Models
{
	using System.ComponentModel.DataAnnotations;

	public class InstallModel
	{
		//
		//  GIT ACCESS
		//
		[Required]
		[Display(Name = "Wiki Repository")]
		public string WikiRepository { get; set; }

		[Required]
		[Display(Name = "Git Username")]
		public string GitUser { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Git Password")]
		public string GitPassword { get; set; }

		[Required]
		[Display(Name = "Use this git account for all users")]
		public bool UseSameGitAccountForAllUsers;

		//
		// DATABASE
		//
		[Required]
		[Display(Name = "Database Provider")]
		public string DatabaseProvider { get; set; }

		[Required]
		[Display(Name = "Database Connection String")]
		public string ConnectionString { get; set; }

		//
		//  ADMINISTRATION
		//

		[Required]
		[Display(Name = "Admin Username")]
		public string AdminUsername { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Admin Password")]
		public string AdminPassword { get; set; }

		[UIHint("Checkbox")]
		[Display(Name = "Allow Registration")]
		public bool AllowRegistration { get; set; }

		[UIHint("Checkbox")]
		[Display(Name = "Enable Login with Google")]
		public bool EnableGoogleLogin { get; set; }

		[Display(Name = "Google Apps Domain")]
		public string GoogleAppsDomain { get; set; }

		public InstallModel()
		{
			AdminUsername = "admin";
			DatabaseProvider = "System.Data.SqlClient";
		}
	}
}