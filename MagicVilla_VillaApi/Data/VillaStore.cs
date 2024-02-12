using MagicVilla_VillaApi.Models.Dto;

namespace MagicVilla_VillaApi.Data
{
	public static class VillaStore
	{
		public static List<VillaDTO> villaList = new List<VillaDTO> {
			new VillaDTO { Id = 1, Name = "Pool View", Occupancy = 3, Sqft = 100 },
			new VillaDTO { Id = 2, Name = "Beach View", Occupancy = 4, Sqft = 300 }
			};
	}
}