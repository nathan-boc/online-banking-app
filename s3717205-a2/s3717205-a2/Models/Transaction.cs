using System.ComponentModel.DataAnnotations;

namespace MvcBank.Models;

public class Transaction 
{
	public int TransactionID { get; set; }

	[Required]
	[RegularExpression("^[DWTSB]{1}", ErrorMessage = "Must be a valid transaction type (D/W/T/S/B).")]
	public char TransactionType { get; set; }

	[Required]
	// TODO : Add Account foreign key annotation here
	public int AccountNumber { get; set; }

	// TODO : Add Account foreign key annotation here
	public int DestinationAccountNumber { get; set; }

	[Required]
	[DataType(DataType.Currency), Range(0.0, Double.MaxValue)]
	public decimal Amount { get; set; }

	[MaxLength(30)]
	public string Comment { get; set; }

	[Required]
	public DateTime TransactionTimeUtc { get; set; }
}
