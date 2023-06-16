using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlueBrown.BigBola.Api.Controllers
{
	[ApiController]
	public class WalletActionController : ControllerBase
	{
		private readonly IRepository _repository;

		public WalletActionController(IRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		[ApiKey]
		[Route("~/files/web/kyc/search")]
		public async Task<IReadOnlyCollection<WalletAction>> GetWalletActions(
			[FromQuery] string startDate,
			[FromQuery] string endDate,
			[FromQuery] int rows,
			[FromQuery] int page)
		{
			var request = new Request(startDate, endDate, rows, page);

			return await _repository.ReadWalletActions(request);
		}
	}
}
