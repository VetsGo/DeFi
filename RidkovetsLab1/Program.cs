using Nethereum.Signer;

namespace RidkovetsLab1;

class Program
{
    static void Main(string[] args)
    {
        string targetPrefix = "0x10"; // Число дня народження: 10

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
        Console.WriteLine($"Private key:  {ecKey.GetPrivateKey()}");
    }
}