namespace MvcBank.Models;

public class BillPay 
{
	public int BillPayID { get; set; }
	public int AccountNumber { get; set; }
	public int PayeeID { get; set; }
	public decimal Amount { get; set; }
	public DateTime ScheduleTImeUtc { get; set; }
	public char Period { get; set; }
}
