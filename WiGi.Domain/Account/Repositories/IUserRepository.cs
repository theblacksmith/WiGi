namespace WiGi.Account.Repositories
{
	public interface IUserRepository : IRepository<User>
	{
		User FindByUsername(string username);
	}
}
