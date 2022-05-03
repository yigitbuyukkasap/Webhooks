using AutoMapper;
using AirlineWeb.Dtos;
using AirlineWeb.Models;

namespace AirlineWeb.Profiles
{
    public class FlightDetailProfile : Profile
    {
        public FlightDetailProfile()
        {
            // Source --> Target            
            CreateMap<FlightDetailCreateDto, FlightDetail>();
            CreateMap<FlightDetail, FlightDetailReadDto>();
            CreateMap<FlightDetailUpdateDto, FlightDetail>();
        }
    }
}