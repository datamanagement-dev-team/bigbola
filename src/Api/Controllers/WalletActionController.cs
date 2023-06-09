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
		public async Task<IReadOnlyCollection<WalletAction>> GetWalletActions([FromQuery] Request request)
		{
			return await _repository.ReadWalletActions(request);
		}
	}
}
