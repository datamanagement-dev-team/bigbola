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
		[Route("[action]")]
		public async Task<IReadOnlyCollection<WalletAction>> GetWalletActions(
			[FromQuery] DateTime start_date, [FromQuery] DateTime end_date, 
			[FromQuery] int rows, [FromQuery] int page)
		{
			return await _repository.ReadWalletActions(start_date, end_date, rows, page);
		}
	}
}
