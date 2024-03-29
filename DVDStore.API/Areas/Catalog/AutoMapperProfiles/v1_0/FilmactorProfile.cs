﻿using AutoMapper;
using DVDStore.Common.Models.v1_0.Dto;
using DVDStore.DAL.Models;

namespace DVDStore.API.Areas.Catalog.AutoMapperProfiles.v1_0
{
	/// <summary>
	///     FilmactorProfile
	/// </summary>
	public class FilmactorProfile : Profile
	{
		#region Public Constructors

		/// <summary>
		///   FilmactorProfile Constructor
		/// </summary>
		public FilmactorProfile()
		{
			CreateMap<Filmactor, FilmactorDto>();
		}

		#endregion Public Constructors
	}
}