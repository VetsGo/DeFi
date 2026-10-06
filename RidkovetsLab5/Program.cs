using System.Numerics;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using ReikovetsLab5;

namespace RidkovetsLab5;

class Program
{
    private const string RpcUrl = "http://127.0.0.1:8545";
    private const string PrivateKey = "0x133941029bc3d748514a0b44d441816275ddf573fdd734962c48b5586ec07146";

    private const string StableEngineAddress = "0xe0861bF53521e727Cb232f68cCe9D75590C25C70";
    
    static async Task Main(string[] args)
    {
        var account = new Account(PrivateKey);
        var web3 = new Web3(account, RpcUrl);

        Console.WriteLine($"User: {account.Address}");

        try
        {
            Console.WriteLine("\n[Step 1] Deposit 2 ETH");
            var depositTx = new DepositCollateralFunction
            {
                AmountToSend = Web3.Convert.ToWei(2)
            };
            var depositReceipt = await web3.Eth.GetContractTransactionHandler<DepositCollateralFunction>()
                .SendRequestAndWaitForReceiptAsync(StableEngineAddress, depositTx);
            Console.WriteLine($"2 ETH successfully deposited. Hash: {depositReceipt.TransactionHash}");
            
            var getMaxMintableFunction = new GetMaxMintableAmountFunction { User = account.Address };
            var maxMintable = await web3.Eth.GetContractQueryHandler<GetMaxMintableAmountFunction>()
                .QueryAsync<BigInteger>(StableEngineAddress, getMaxMintableFunction);

            Console.WriteLine($"\n[Step 2] Maximum possible minting amount: {Web3.Convert.FromWei(maxMintable)} rUSD");
            
            var mintTx = new MintStablecoinFunction
            {
                SUsdAmount = maxMintable
            };
            var mintReceipt = await web3.Eth.GetContractTransactionHandler<MintStablecoinFunction>()
                .SendRequestAndWaitForReceiptAsync(StableEngineAddress, mintTx);
            Console.WriteLine($"Successfully released {Web3.Convert.FromWei(maxMintable)} rUSD. Hash: {mintReceipt.TransactionHash}");
            
            Console.WriteLine("\n[Step 3] Attempt to withdraw 1 ETH of collateral");
            var withdrawTx = new WithdrawCollateralFunction
            {
                Amount = Web3.Convert.ToWei(1)
            };

            await web3.Eth.GetContractTransactionHandler<WithdrawCollateralFunction>()
                .SendRequestAndWaitForReceiptAsync(StableEngineAddress, withdrawTx);

            Console.WriteLine("Error: The transaction went through!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n==================================================");
            Console.WriteLine("The protection worked successfully!");
            Console.WriteLine($"Transaction reversal intercepted: {ex.Message}");
            Console.WriteLine("The attempt to lift the lien was blocked");
            Console.WriteLine("==================================================");
        }
    }
}