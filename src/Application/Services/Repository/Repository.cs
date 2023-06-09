using BlueBrown.BigBola.Application.Entities;

namespace BlueBrown.BigBola.Application.Services.Repository
{
	public interface IRepository
	{
		Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(string startDate, string endDate, int rows, int pag);
	}
}
