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

            // BillPay items will be checked for every minute
            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }

    private async Task ExecuteBillPay(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MvcBankContext>();

        // Looks for bill pays with their schedule time in the past
        var scheduledBills = await context.BillPay.Where(x => x.ScheduleTimeUtc <= DateTime.UtcNow).ToListAsync(cancellationToken);

        // Execute each scheduled bill
        foreach (var bill in scheduledBills)
        {
            var account = await context.Account.FindAsync(bill.AccountNumber);

            // Checks if account has sufficient funds
            if ((account.AccountType == 'C' && account.Balance - bill.Amount < 300)
                || (account.AccountType == 'S' && account.Balance - bill.Amount < 0))
            {
                // Deletes the bill if the payment could not be afforded

            }
            else
            {
                // Decrease the account balance by the payment ammount

                // Add transaction of type B

                // Update BillPay row based on Period S / M .AddMonths(1)
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

