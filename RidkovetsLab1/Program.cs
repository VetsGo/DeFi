using Nethereum.Web3;

namespace RidkovetsLab1;

class Program
{
    static async Task Main(string[] args)
    {
        string rpcUrl = "https://eth-sepolia.g.alchemy.com/v2/alch_ohPjfg0J-A371o42lIuj8";
        var web3 = new Web3(rpcUrl);
        
        string myAddress = "0x102Fdd17C36Ec1235C0f82945f842Eb02A1cEC3F"; 

        Console.WriteLine($"Retrieving the balance for an address: {myAddress}");
        
        var balanceWei = await web3.Eth.GetBalance.SendRequestAsync(myAddress);
        decimal balanceEther = Web3.Convert.FromWei(balanceWei.Value);

        Console.WriteLine($"Balance in Wei:   {balanceWei.Value}");
        Console.WriteLine($"Balance in Ether: {balanceEther} ETH");
    }
}