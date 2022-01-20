namespace MvcBank.Models;

public class Account
{
	// TODO : ensure this is set to the primary key
	public int AccountNumber { get; set; }
	public char AccountType { get; set; }
	public int CustomerID { get; set; }
}
