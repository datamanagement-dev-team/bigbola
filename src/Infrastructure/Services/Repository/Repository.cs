using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Repository;


namespace BlueBrown.BigBola.Infrastructure.Services.Repository
{
	internal class Repository : IRepository
	{
		public async Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(DateTime start_date, DateTime end_date, int rows, int page)
		{
			//test
			return default;
		}
	}
}
