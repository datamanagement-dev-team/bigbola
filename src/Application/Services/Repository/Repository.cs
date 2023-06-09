using BlueBrown.BigBola.Application.Entities;

namespace BlueBrown.BigBola.Application.Services.Repository
{
	public interface IRepository
	{
		Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(DateTime start_date, DateTime end_date, int rows, int pag);
	}
}
