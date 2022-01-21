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
}