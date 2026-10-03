using Nethereum.Web3;

namespace RidkovetsLab3;

public class Web3IndexerService : BackgroundService
{
    private readonly string _rpcUrl = "http://127.0.0.1:8545"; 
    private readonly string _contractAddress = "0x2FbB4de7c6dB1C932DA408E8cB9172bA3E08C3a0"; 

    public static List<SwapRecord> SwapDb = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var web3 = new Web3(_rpcUrl);
        var swapEvent = web3.Eth.GetEvent<SwapEventDTO>(_contractAddress);
        var filter = await swapEvent.CreateFilterAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var logs = await swapEvent.GetFilterChangesAsync(filter);
                foreach (var log in logs)
                {
                    SwapDb.Add(new SwapRecord
                    {
                        TransactionHash = log.Log.TransactionHash,
                        Trader = log.Event.Trader,
                        AmountIn = log.Event.AmountIn.ToString(),
                        AmountOut = log.Event.AmountOut.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading RPC: {ex.Message}");
            }
            await Task.Delay(3000, stoppingToken);
        }
    }
}