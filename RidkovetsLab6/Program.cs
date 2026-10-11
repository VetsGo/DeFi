using System.Numerics;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace RidkovetsLab6;

class Program
{
    private const string RpcUrl = "https://eth-sepolia.g.alchemy.com/v2/alch_ohPjfg0J-A371o42lIuj8";
    private const string LiquidatorPrivateKey = "0x5bd4bb9908ec61b38be4cbf84f9a5edd7b3a7c7eaa678d8ac39db353c36892a0"; 
    private const string StableEngineAddress = "0x96B6b548b5441E152096fa69b4745bb7a5b6a467";

    static async Task Main(string[] args)
    {
        var account = new Account(LiquidatorPrivateKey);
        var web3 = new Web3(account, RpcUrl);
        web3.Eth.TransactionManager.UseLegacyAsDefault = true;

        Console.WriteLine($"Бот-ліквідатор запущено. Гаманець: {account.Address}");

        string[] usersToMonitor = { "0x23b2DF50e2E49d14524bED8c9493D024a99fb22F" };

        while (true)
        {
            foreach (var user in usersToMonitor)
            {
                try
                {
                    var healthFactorQuery = new GetHealthFactorFunction { User = user };
                    var queryHandler = web3.Eth.GetContractQueryHandler<GetHealthFactorFunction>();

                    var healthFactorWei =
                        await queryHandler.QueryAsync<BigInteger>(StableEngineAddress, healthFactorQuery);
                    decimal healthFactor = Web3.Convert.FromWei(healthFactorWei);

                    Console.WriteLine($"[Моніторинг] Позиція: {user} | Health Factor: {healthFactor}");

                    if (healthFactor < 1.0m)
                    {
                        Console.WriteLine($"[УВАГА] Виявлено токсичну позицію {user}! Починаємо ліквідацію...");

                        var liquidateTx = new LiquidateFunction { User = user };
                        var txHandler = web3.Eth.GetContractTransactionHandler<LiquidateFunction>();

                        var receipt =
                            await txHandler.SendRequestAndWaitForReceiptAsync(StableEngineAddress, liquidateTx);

                        Console.WriteLine($"[УСПІХ] Позицію успішно ліквідовано!");
                        Console.WriteLine($"Блок: {receipt.BlockNumber} | Tx Hash: {receipt.TransactionHash}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка перевірки позиції {user}: {ex.Message}");
                }
            }

            await Task.Delay(12000);
        }
    }
}