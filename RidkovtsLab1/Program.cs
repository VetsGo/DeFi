using Nethereum.Signer;

namespace RidkovtsLab1;

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

        Console.WriteLine($"\nЗнайдено гаманець після {attempts} спроб!");
        Console.WriteLine($"Публічна адреса: {address}");
        Console.WriteLine($"Приватний ключ:  0x{ecKey.GetPrivateKey()}");
    }
}