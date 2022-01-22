using System.ComponentModel.DataAnnotations;

namespace MvcBank.Models;

public class Account
{
	[Key]
	public int AccountNumber { get; set; }
	public char AccountType { get; set; }
	public int CustomerID { get; set; }
}
