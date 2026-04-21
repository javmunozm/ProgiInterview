using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappings;

public sealed class BidCalculationProfile : Profile
{
    public BidCalculationProfile()
    {
        CreateMap<Fee, FeeDto>();

        CreateMap<BidCalculation, BidCalculationDto>()
            .ForMember(d => d.VehicleType, opt => opt.MapFrom(s => s.VehicleType.ToString()));
    }
}
