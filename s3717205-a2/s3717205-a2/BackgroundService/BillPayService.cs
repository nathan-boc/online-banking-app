using Microsoft.EntityFrameworkCore;
using MvcBank.Data;
using MvcBank.Models;

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
                context.BillPay.Remove(bill);
            }
            else
            {
                // Decrease the account balance by the payment amount
                account.Balance -= bill.Amount;

                // Add transaction of type B
                account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = 'B',
                        Amount = bill.Amount,
                        TransactionTimeUtc = DateTime.UtcNow
                    });

                // Update BillPay row based on Period S / M
                if(bill.Period == 'S')
                {
                    // Deletes the completed billpay
                    context.BillPay.Remove(bill);
                }
                else if(bill.Period == 'M')
                {
                    // Adds a month to reschedule the bill
                    bill.ScheduleTimeUtc.AddMonths(1);
                }               
            }
            // Save changes to the database context
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}

