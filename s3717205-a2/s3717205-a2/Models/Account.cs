using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MvcBank.Models;

public class Account
{
	[Key, Required]
	[MaxLength(4), MinLength(4)]
	public int AccountNumber { get; set; }

	[Required]
	[RegularExpression("^[CS]{1}", ErrorMessage = "Must be either 'C' for Checking or 'S' for Savings.")]
	public char AccountType { get; set; }

	[Required]
	// TODO : Add Customer foreign key annotation here
	public int CustomerID { get; set; }
}
