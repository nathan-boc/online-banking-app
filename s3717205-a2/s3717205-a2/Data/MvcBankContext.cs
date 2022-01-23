using Microsoft.EntityFrameworkCore;
using MvcBank.Models;

namespace MvcBank.Data;

public class MvcBankContext : DbContext
{
	public MvcBankContext(DbContextOptions<MvcBankContext> options) : base(options)
	{ }

	public DbSet<Customer> Customer { get; set; }
	public DbSet<Login> Login { get; set; }
	public DbSet<Account> Account { get; set; }
	public DbSet<Transaction> Transaction { get; set; }
	public DbSet<BillPay> BillPay { get; set; }
	public DbSet<Payee> Payee { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
        base.OnModelCreating(builder);

		// Check constraints for Customer
		builder.Entity<Customer>().HasCheckConstraint("CH_Customer_CustomerID", "len(CustomerID) = 4")
			.HasCheckConstraint("CH_Customer_Name", "len(Name) <= 50");

		// Check constraints for Login
		builder.Entity<Login>().HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8")
			.HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");

		// Check constraints for Account
		builder.Entity<Account>().HasCheckConstraint("CH_Account_AccountNumber", "len(AccountNumber) = 4")
			.HasCheckConstraint("CH_Account_AccountType", "AccountType in ('C', 'S')")
			.HasCheckConstraint("CH_Account_Balance", "Balance >= 0");

		// Check constraints for Transaction
		builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_TransactionType", "TransactionType in ('D', 'W', 'T', 'S', 'B')")
			.HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");

		// Check constraints for BillPay
		builder.Entity<BillPay>().HasCheckConstraint("CH_BillPay_Amount", "Amount > 0")
			.HasCheckConstraint("CH_BillPay_Period", "Period in ('O', 'M')");

		// Check constraints for Payee
		builder.Entity<Payee>().HasCheckConstraint("CH_Payee_Name", "len(Name) <= 50")
			.HasCheckConstraint("CH_Payee_Address", "len(Address) <= 50")
			.HasCheckConstraint("CH_Payee_Suburb", "len(Suburb) <= 50")
			.HasCheckConstraint("CH_Payee_State", "State in ('VIC', 'SA', 'WA', 'QLD', 'TAS', 'NT', 'NSW', 'ACT')");
	}
}	