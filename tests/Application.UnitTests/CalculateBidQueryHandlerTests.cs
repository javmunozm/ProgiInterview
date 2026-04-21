using Application.DTOs;
using Application.Mappings;
using Application.Queries.CalculateBid;
using AutoMapper;
using FluentAssertions;

namespace Application.UnitTests;

public class CalculateBidQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly CalculateBidQueryHandler _handler;

    public CalculateBidQueryHandlerTests()
    {
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<BidCalculationProfile>();
        var config = new MapperConfiguration(expression, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
        _handler = new CalculateBidQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_CommonVehicle_ReturnsCorrectDto()
    {
        var query = new CalculateBidQuery(1000m, "Common");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.VehiclePrice.Should().Be(1000m);
        result.VehicleType.Should().Be("Common");
        result.TotalPrice.Should().Be(1180m);
        result.Fees.Should().HaveCount(4);
    }

    [Fact]
    public async Task Handle_LuxuryVehicle_ReturnsCorrectDto()
    {
        var query = new CalculateBidQuery(1800m, "Luxury");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalPrice.Should().Be(2167m);
    }

    [Fact]
    public async Task Handle_CaseInsensitiveVehicleType()
    {
        var query = new CalculateBidQuery(1000m, "common");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.VehicleType.Should().Be("Common");
        result.TotalPrice.Should().Be(1180m);
    }

    [Fact]
    public void AutoMapperConfiguration_IsValid()
    {
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<BidCalculationProfile>();
        var config = new MapperConfiguration(expression, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance);
        config.AssertConfigurationIsValid();
    }
}
