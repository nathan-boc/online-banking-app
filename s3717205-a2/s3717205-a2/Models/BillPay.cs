using System.ComponentModel.DataAnnotations;

namespace MvcBank.Models;

public class BillPay 
{
	public int BillPayID { get; set; }

	[Required]
	// TODO : Add Account foreign key annotation here
	public int AccountNumber { get; set; }

	[Required]
	// TODO : Add Payee foreign key annotation here
	public int PayeeID { get; set; }

	[Required]
	[DataType(DataType.Currency), Range(0.0, Double.MaxValue)]
	public decimal Amount { get; set; }

	[Required]
	public DateTime ScheduleTImeUtc { get; set; }

	[Required]
	[RegularExpression("^[OM]{1}", ErrorMessage = "Must be either 'O' for One-off or 'M' for Monthly.")]
	public char Period { get; set; }
}
