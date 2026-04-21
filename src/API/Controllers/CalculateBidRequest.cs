using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

public sealed class CalculateBidRequest
{
    [Required(ErrorMessage = "Vehicle price is required.")]
    public decimal? VehiclePrice { get; set; }

    [Required(ErrorMessage = "Vehicle type is required.")]
    public string? VehicleType { get; set; }
}
