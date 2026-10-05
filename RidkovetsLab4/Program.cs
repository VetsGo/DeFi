using System.Numerics;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;

namespace RidkovetsLab4;

class Program
{
    static async Task Main(string[] args)
    {
            string rpcUrl = "https://eth-sepolia.g.alchemy.com/v2/alch_ohPjfg0J-A371o42lIuj8";
            string privateKey = "0x5bd4bb9908ec61b38be4cbf84f9a5edd7b3a7c7eaa678d8ac39db353c36892a0"; 
            
            string tokenAAddress = "0xbae16B25C0334cf4A1BAB0837D6e562503BFc4e9";
            string tokenBAddress = "0x43028F03e30C8A2D680331B72601C3997D1551CA";
            string integratorAddress = "0x85ad108334b85031DC98eb6399a49710020bBCfA";
            
            var account = new Account(privateKey);
            var web3 = new Web3(account, rpcUrl);

        
            string erc20Abi = @"[{""constant"":false,""inputs"":[{""name"":""spender"",""type"":""address""},{""name"":""amount"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""name"":"""",""type"":""bool""}],""type"":""function""}]";
            
            string integratorAbi = @"[
                {""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""},{""internalType"":""uint256"",""name"":""amountADesired"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBDesired"",""type"":""uint256""}],""name"":""provideLiquidity"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},
                {""inputs"":[{""internalType"":""address"",""name"":""tokenIn"",""type"":""address""},{""internalType"":""address"",""name"":""tokenOut"",""type"":""address""},{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""}],""name"":""swapTokens"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""}
            ]";

            var tokenAContract = web3.Eth.GetContract(erc20Abi, tokenAAddress);
            var tokenBContract = web3.Eth.GetContract(erc20Abi, tokenBAddress);
            var integratorContract = web3.Eth.GetContract(integratorAbi, integratorAddress);
            
            BigInteger liquidityAmount = Web3.Convert.ToWei(100000); 
            BigInteger swapAmount = Web3.Convert.ToWei(1000);       

            Console.WriteLine("=== a. Granting approval to the integrator contract ===");
            var approveAFn = tokenAContract.GetFunction("approve");
            var approveBFn = tokenBContract.GetFunction("approve");
            
            var txA = await approveAFn.SendTransactionAsync(account.Address, new HexBigInteger(500000), null, integratorAddress, liquidityAmount + swapAmount);
            Console.WriteLine($"The approval for Token A has been sent. Tx Hash: {txA}");
            
            var txB = await approveBFn.SendTransactionAsync(account.Address, new HexBigInteger(500000), null, integratorAddress, liquidityAmount);
            Console.WriteLine($"The approval for Token B has been sent. Tx Hash: {txB}");
            
            Console.WriteLine("Waiting for network confirmation...");
            await Task.Delay(20000);


            Console.WriteLine("\n=== b. provideLiquidity call ===");
            var provideLiquidityFn = integratorContract.GetFunction("provideLiquidity");
            var txLiq = await provideLiquidityFn.SendTransactionAsync(account.Address, new HexBigInteger(3000000), null, tokenAAddress, tokenBAddress, liquidityAmount, liquidityAmount);
            Console.WriteLine($"Liquidity Tx Hash: {txLiq}");
            
            Console.WriteLine("Waiting for network confirmation...");
            await Task.Delay(20000);


            Console.WriteLine("\n=== c. swapTokens call ===");
            var swapTokensFn = integratorContract.GetFunction("swapTokens");
            BigInteger amountOutMin = 1;
            var txSwap = await swapTokensFn.SendTransactionAsync(account.Address, new HexBigInteger(3000000), null, tokenAAddress, tokenBAddress, swapAmount, amountOutMin);
            Console.WriteLine($"Swap Tx Hash: {txSwap}");

            Console.WriteLine("\nTask has been completed!");
    }
}