using AutoMapper;
using Copy_1_NationalParkAPI_1031.Models;
using Copy_1_NationalParkAPI_1031.Models.DTOs;

namespace Copy_1_NationalParkAPI_1031.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<TrailDto, Trail>().ReverseMap();
        }
    }
}
// DB--MODEL--REP--DTO-ClIENT
//CLIENT--DTO--MODEL--DB