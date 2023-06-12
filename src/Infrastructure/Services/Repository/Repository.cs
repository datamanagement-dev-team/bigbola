using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Repository;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BlueBrown.BigBola.Infrastructure.Services.Repository
{
	public class Repository : IRepository
	{
		public async Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(string startDate, string endDate, int rows, int page)
		{
			//todo connectionstring
            using var db = new SqlConnection("Data Source=devgr-rpt-01.devgr-novibet.systems;Initial Catalog=BBReporting;Transaction Binding=Explicit Unbind;Persist Security Info=True;User ID=sa;Password=p@ssw0rd;Connect Timeout=60;Encrypt=True;TrustServerCertificate=True");

            await db.QuerySingleAsync<int>("SELECT 1");
            return default;
		}
	}
}
