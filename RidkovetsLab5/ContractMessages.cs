using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ReikovetsLab5
{
    [Function("depositCollateral")]
    public class DepositCollateralFunction : FunctionMessage { }

    [Function("getMaxMintableAmount", "uint256")]
    public class GetMaxMintableAmountFunction : FunctionMessage
    {
        [Parameter("address", "user", 1)]
        public string User { get; set; }
    }

    [Function("mintStablecoin")]
    public class MintStablecoinFunction : FunctionMessage
    {
        [Parameter("uint256", "_sUsdAmount", 1)]
        public BigInteger SUsdAmount { get; set; }
    }

    [Function("withdrawCollateral")]
    public class WithdrawCollateralFunction : FunctionMessage
    {
        [Parameter("uint256", "_amount", 1)]
        public BigInteger Amount { get; set; }
    }
}