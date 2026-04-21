using Application.DTOs;
using Application.Queries.CalculateBid;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class BidController : ControllerBase
{
    private readonly IMediator _mediator;

    public BidController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("calculate")]
    [OutputCache(PolicyName = "BidCalculation")]
    [ProducesResponseType(typeof(BidCalculationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<BidCalculationDto>> Calculate(
        [FromQuery] CalculateBidRequest request,
        CancellationToken cancellationToken)
    {
        var query = new CalculateBidQuery(request.VehiclePrice!.Value, request.VehicleType!);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
