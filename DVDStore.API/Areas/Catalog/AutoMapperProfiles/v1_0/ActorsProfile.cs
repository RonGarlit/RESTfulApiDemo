// /**********************************************************************************
// **
// **  RESTfulApiPrototype v1.0
// **
// **  Copyright 2020
// **  Developed by:  Ronald A. Garlit .
// **
// ***********************************************************************************
// **
// **  FileName: ActorsProfile.cs (DVDStore.API)
// **  Version: 0.1
// **  Author: Ronald A. Garlit
// **
// **  Description:
// **
// **  Actors AutoMapper Profile Class.
// **
// **  Change History
// **
// **  WHEN        WHO         WHAT
// **---------------------------------------------------------------------------------
// **  2020-11-16  rgarlit     STARTED DEVELOPMENT
// ***********************************************************************************/

using AutoMapper;
using DVDStore.Common.Models.v1_0.Dto;
using DVDStore.DAL.Models;

namespace DVDStore.API.Areas.Catalog.AutoMapperProfiles.v1_0
{
    /// <summary>
    ///     ActorsProfile
    /// </summary>
    public class ActorsProfile : Profile
    {
        #region Public Constructors

        /// <summary>
        ///     ActorsProfile Constructor
        /// </summary>
        /// <remarks>
        ///     AutoMapper docs - Projection and CreateMap
        ///     https://docs.automapper.org/en/stable/Projection.html
        /// </remarks>
        public ActorsProfile()
        {
            CreateMap<Actor, ActorDto>()
                // Map the FullName property of DTO from Dal Entity Model
                .ForMember(dest => dest.FullName,
                    src => src.MapFrom(src => $"{src.Firstname} {src.Lastname}"));
            CreateMap<ActorForCreationDto, Actor>();
            CreateMap<ActorForUpdateDto, Actor>();
        }

        #endregion Public Constructors
    }
}