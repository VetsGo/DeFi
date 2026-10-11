using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace RidkovetsLab6
{
    [Function("getHealthFactor", "uint256")]
    public class GetHealthFactorFunction : FunctionMessage
    {
        [Parameter("address", "user", 1)]
        public string User { get; set; }
    }

    [Function("liquidate")]
    public class LiquidateFunction : FunctionMessage
    {
        [Parameter("address", "user", 1)]
        public string User { get; set; }
    }

    [Function("reduceCollateralForTesting")]
    public class ReduceCollateralFunction : FunctionMessage
    {
        [Parameter("address", "user", 1)]
        public string User { get; set; }

        [Parameter("uint256", "amount", 2)]
        public BigInteger Amount { get; set; }
    }
}