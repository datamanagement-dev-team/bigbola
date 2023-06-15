using BlueBrown.BigBola.Application;
using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Repository;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BlueBrown.BigBola.Infrastructure.Services.Repository
{
	public class Repository : IRepository
	{

        private readonly ISettings _settings;

        public Repository(ISettings settings)
        {
            _settings = settings;
        }

        public async Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(Request request)
		{
            using var db = new SqlConnection(_settings.ReportingConnectionString);

            await db.QuerySingleAsync<int>("SELECT 1");

            return new List<WalletAction>();
		}
	}
}
