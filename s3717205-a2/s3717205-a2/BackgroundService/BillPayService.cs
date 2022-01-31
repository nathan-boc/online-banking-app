using Microsoft.EntityFrameworkCore;
using MvcBank.Data;

namespace MvcBank.BackgroundServices;

public class BillPayService : BackgroundService
{
    private readonly IServiceProvider _services;

    public BillPayService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await ExecuteBillPay(cancellationToken);

            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }

    private async Task ExecuteBillPay(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MvcBankContext>();

        // Looks for bill pays with their schedule time in the past
        var persons = await context.BillPay.Where(x => x.ScheduleTimeUtc <= DateTime.UtcNow).ToListAsync(cancellationToken);

        // TODO : Delete bill pay row if payment couldnt be afforded

        // TODO: Decrease account balance by the amount and add transaction type B

        // TODO : If period is S, delete row. If period is M, add one month to the schedule time - .AddMonths(1)

        await context.SaveChangesAsync(cancellationToken);
    }
}

