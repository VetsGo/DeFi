using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace RidkovetsLab1;

class Program
{
    static async Task Main(string[] args)
    {
        //--------Wallet generator--------
        /*string targetPrefix = "0x10"; // Число дня народження: 10

        EthECKey ecKey;
        string address;
        int attempts = 0;

        do
        {
            attempts++;
            ecKey = EthECKey.GenerateKey();
            address = ecKey.GetPublicAddress();
        } 
        while (!address.StartsWith(targetPrefix, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine($"\nWallet found after {attempts} attempts!");
        Console.WriteLine($"Public address: {address}");
        Console.WriteLine($"Private key:  {ecKey.GetPrivateKey()}");*/
        
        string rpcUrl = "https://eth-sepolia.g.alchemy.com/v2/alch_ohPjfg0J-A371o42lIuj8";

        // Приватний ключ ПЕРШОГО гаманця
        string senderPrivateKey = "0x4cb31782737cab43711578ea9ac4a15883a9f40c59f31ef8a95a428ef6feee88";

        // Публічна адреса ДРУГОГО гаманця
        string recipientAddress = "0x103b9b1B1C397A7c0C1feE1Ca9d2B595ff2871F4";
        
        var account = new Account(senderPrivateKey);
        var web3 = new Web3(account, rpcUrl);

        Console.WriteLine($"Sender (Wallet 1): {account.Address}");
        Console.WriteLine($"Recipient (Wallet 2): {recipientAddress}\n");
        
        var initialBalance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
        Console.WriteLine($"Sender's opening balance: {Web3.Convert.FromWei(initialBalance.Value)} ETH\n");
        
        decimal amountToSendInEther = 0.001m; // Сума переказу в ETH

        Console.WriteLine($"Sending {amountToSendInEther} ETH...");

        // Метод підписання та відправлення транзакції + квитанція в блоці
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