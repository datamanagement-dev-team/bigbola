namespace BlueBrown.BigBola.Application.Services.Repository
{
	public interface IRepository
	{
		Task<object> ReadWalletActions();
	}
}
