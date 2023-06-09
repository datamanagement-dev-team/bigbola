using BlueBrown.BigBola.Application.Entities;

namespace BlueBrown.BigBola.Application.Services.Repository
{
	public interface IRepository
	{
		Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(Request request);
	}
}
