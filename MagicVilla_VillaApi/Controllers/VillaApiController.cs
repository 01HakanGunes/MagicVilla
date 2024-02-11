using MagicVilla_VillaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaApi.Controllers
{
	[ApiController]
	public class VillaApiController : ControllerBase
	{
		public IEnumerable<Villa> GetVillas()
		{
			return new List<Villa> { };
		}
	}
}