namespace Auto.WebAPI.Services.Interfaces;

interface IPrintService
{
    Task<bool> Print(CancellationToken ct);
}
