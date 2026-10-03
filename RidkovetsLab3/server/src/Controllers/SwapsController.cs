using Microsoft.AspNetCore.Mvc;

namespace RidkovetsLab3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SwapsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSwaps([FromQuery] string? trader)
    {
        if (string.IsNullOrEmpty(trader))
        {
            return Ok(Web3IndexerService.SwapDb);
        }

        var result = Web3IndexerService.SwapDb
            .Where(s => s.Trader.Equals(trader, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(result);
    }
}