namespace WiGi.Data.Account.Repositories
{
	using System.Linq;
	using WiGi.Account;
	using WiGi.Account.Repositories;
	using Wiki;

	public class UserRepository : Repository<WiGiCtx, User>, IUserRepository
	{
		public UserRepository(WiGiCtx ctx)
			: base(ctx)
		{
		}

		public User FindByUsername(string username)
		{
			using (var ctx = new WiGiCtx())
			{
				return ctx.Users.FirstOrDefault(u => u.Username == username);
			}
		}
	}
}
