using AutoMapper;
using DVDStore.Common.Models.v2_0.Dto;
using DVDStore.DAL.Models;

namespace DVDStore.API.Areas.Catalog.AutoMapperProfiles.v2_0
{
	/// <summary>
	///     FilmsProfile
	/// </summary>
	public class FilmsProfile : Profile
	{
		#region Public Constructors

		/// <summary>
		///     FilmsProfile Constructor
		/// </summary>
		public FilmsProfile()
		{
			CreateMap<Film, FilmDto>();
			CreateMap<FilmForCreationDto, Film>();
		}

		#endregion Public Constructors
	}
}