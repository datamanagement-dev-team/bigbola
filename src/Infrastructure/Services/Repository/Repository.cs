using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Repository;


namespace BlueBrown.BigBola.Infrastructure.Services.Repository
{
	internal class Repository : IRepository
	{
		public async Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(string startDate, string endDate, int rows, int page)
		{
			//test
			return default;
		}
	}
}
