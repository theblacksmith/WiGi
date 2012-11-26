namespace WiGi.Account
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool IsAdmin { get; set; }

		public User()
		{
			IsAdmin = false;
		}
	}
}