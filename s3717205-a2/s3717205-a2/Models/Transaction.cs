using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBank.Models;

public class Transaction 
{
	public int TransactionID { get; set; }

	[Required, RegularExpression("^[DWTSB]{1}", ErrorMessage = "Must be a valid transaction type (D/W/T/S/B).")]
	public char TransactionType { get; set; }

	[ForeignKey(nameof(Account))]
	public int AccountNumber { get; set; }
	public virtual Account Account { get; set; }

	[ForeignKey(nameof(DestinationAccount))]
	public int? DestinationAccountNumber { get; set; }
	public virtual Account DestinationAccount { get; set; }

	[Required, Range(0.0, Double.MaxValue)]
	[DataType(DataType.Currency), Column(TypeName = "money")]
	public decimal Amount { get; set; }

	[StringLength(30)]
	public string Comment { get; set; }

	[Required]
	public DateTime TransactionTimeUtc { get; set; }
}
