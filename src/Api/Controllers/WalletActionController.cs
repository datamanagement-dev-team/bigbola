using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlueBrown.BigBola.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class WalletActionController : ControllerBase
	{
		private readonly IRepository _repository;

		public WalletActionController(IRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
        [ApiKey]
        [Route("[action]")]
		public async Task<IReadOnlyCollection<WalletAction>> GetWalletActions(
			[FromQuery] string startDate, [FromQuery] string endDate, 
			[FromQuery] int rows, [FromQuery] int page)
		{
			var request = new Request(startDate, endDate, rows, page);

			//todo discuss if we make a new level for processing the data that the repo returns

			var result = await _repository.ReadWalletActions(request);

			return result;
		}
    }
}
