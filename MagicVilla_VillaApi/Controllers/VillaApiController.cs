using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Logging;
using MagicVilla_VillaApi.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaApi.Controllers
{
	[Route("api/VillaApi")]
	[ApiController]
	public class VillaApiController : ControllerBase
	{
		private readonly ILogging _logger;

		public VillaApiController(ILogging logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			_logger.Log("Getting all villas", "");
			return Ok(VillaStore.villaList);
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<VillaDTO> GetVilla(int id)
		{
			if (id == 0)
			{
				_logger.Log("Got villa error with Id = " + id, "error");
				return BadRequest();
			}

			VillaDTO? villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
			if (villa == null)
			{
				return NotFound();
			}

			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO newVilla)
		{
			if (newVilla == null)
			{
				return BadRequest(newVilla);
			}

			if (newVilla.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			if (VillaStore.villaList.FirstOrDefault(u => u.Name?.ToLower() == newVilla.Name?.ToLower()) != null)
			{
				ModelState.AddModelError("", "Villa already exists!");
				return BadRequest(ModelState);
			}

			newVilla.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault()?.Id + 1 ?? 1;
			VillaStore.villaList.Add(newVilla);

			return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, newVilla);
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult RemoveVilla(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}

			VillaDTO? villaDTO = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

			if (villaDTO == null)
			{
				return NotFound();
			}

			VillaStore.villaList.Remove(villaDTO);

			return NoContent();
		}

		[HttpPut(Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]

		public ActionResult UpdateVilla([FromBody] VillaDTO villaDTO)
		{
			if (villaDTO == null)
			{
				return NotFound();
			}

			VillaDTO? villa = VillaStore.villaList.FirstOrDefault(u => u.Id == villaDTO.Id);

			if (villa == null)
			{
				return BadRequest();
			}

			villa.Name = villaDTO.Name;
			villa.Occupancy = villaDTO.Occupancy;
			villa.Sqft = villaDTO.Sqft;

			return Ok();
		}

		[HttpPatch("{id:int}", Name = "PatchVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult PatchVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
		{
			if (id == 0 || patchDTO == null)
			{
				return BadRequest();
			}

			VillaDTO? villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

			if (villa == null)
			{
				return NotFound();
			}

			patchDTO.ApplyTo(villa, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok();
		}
	}
}