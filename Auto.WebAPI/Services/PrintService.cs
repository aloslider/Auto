using Auto.WebAPI.Services.Interfaces;

namespace Auto.WebAPI.Services;

sealed class PrintService : IPrintService
{
    public async Task<bool> Print(CancellationToken ct)
    {
        int delay = Random.Shared.Next(1, 5);
        await Task.Delay(TimeSpan.FromSeconds(delay), ct);
        return Random.Shared.Next(100) <= 75;
    }
}
