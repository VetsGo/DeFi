using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace RidkovtsLab1;

class Program
{
    static async Task Main(string[] args)
    {
        string rpcUrl = "https://eth-sepolia.g.alchemy.com/v2/alch_ohPjfg0J-A371o42lIuj8";

        // 1. Приватний ключ ПЕРШОГО гаманця
        string senderPrivateKey = "0x0x3f35ff95b1ffb536ee5c865c2e7fe1444eb717e58db806fe3d2a6ae95dd10437";

        // 2. Публічна адреса ДРУГОГО гаманця
        string recipientAddress = "0x108ef741092238B0904eAc5831178afFF5527422";
        
        var account = new Account(senderPrivateKey);
        var web3 = new Web3(account, rpcUrl);

        Console.WriteLine($"Sender (Wallet 1): {account.Address}");
        Console.WriteLine($"Recipient (Wallet 2): {recipientAddress}\n");
        
        var initialBalance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
        Console.WriteLine($"Sender's opening balance: {Web3.Convert.FromWei(initialBalance.Value)} ETH\n");

        decimal amountToSendInEther = 0.001m;

        Console.WriteLine($"Sending {amountToSendInEther} ETH...");
        
        var receipt = await web3.Eth.GetEtherTransferService()
            .TransferEtherAndWaitForReceiptAsync(recipientAddress, amountToSendInEther);
        
        Console.WriteLine("\n----Transaction successfully completed----");
        Console.WriteLine($"Transaction hash: {receipt.TransactionHash}");
        Console.WriteLine($"Block number:              {receipt.BlockNumber.Value}");
        Console.WriteLine($"Gas consumed:           {receipt.GasUsed.Value}");
        Console.WriteLine($"Execution status:         {(receipt.Status.Value == 1 ? "Successfully (1)" : "Error (0)")}");
        
        var senderFinalBalance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
        var recipientFinalBalance = await web3.Eth.GetBalance.SendRequestAsync(recipientAddress);

        Console.WriteLine("\n----Updated balance sheets----");
        Console.WriteLine($"Wallet 1 Balance (Sender): {Web3.Convert.FromWei(senderFinalBalance.Value)} ETH");
        Console.WriteLine($"Wallet 2 Balance (Recipient):  {Web3.Convert.FromWei(recipientFinalBalance.Value)} ETH");
    }
}