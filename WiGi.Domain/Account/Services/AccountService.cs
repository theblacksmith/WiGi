namespace WiGi.Account.Services
{
	using Repositories;

	public class AccountService
	{
		private IUserRepository _usersRepo;

		public AccountService(IUserRepository userRepo)
		{
			_usersRepo = userRepo;
		}
	}
}
