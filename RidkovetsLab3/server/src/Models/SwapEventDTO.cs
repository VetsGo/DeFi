using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace RidkovetsLab3;

[Event("Swap")]
public class SwapEventDTO : IEventDTO
{
    [Parameter("address", "trader", 1, true)]
    public string Trader { get; set; } = string.Empty;

    [Parameter("uint256", "amountIn", 2, false)]
    public BigInteger AmountIn { get; set; }

    [Parameter("uint256", "amountOut", 3, false)]
    public BigInteger AmountOut { get; set; }
}

public class SwapRecord
{
    public string TransactionHash { get; set; } = string.Empty;
    public string Trader { get; set; } = string.Empty;
    public string AmountIn { get; set; } = string.Empty;
    public string AmountOut { get; set; } = string.Empty;
}