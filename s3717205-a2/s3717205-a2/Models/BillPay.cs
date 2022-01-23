using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBank.Models;

public class BillPay 
{
	public int BillPayID { get; set; }

	[ForeignKey(nameof(Account))]
	public int AccountNumber { get; set; }
	public virtual Account Account { get; set; }

	public int PayeeID { get; set; }
	public virtual Payee Payee { get; set; }

	[Required]
	[DataType(DataType.Currency), Range(0.0, Double.MaxValue)]
	public decimal Amount { get; set; }

	[Required]
	public DateTime ScheduleTImeUtc { get; set; }

	[Required]
	[RegularExpression("^[OM]{1}", ErrorMessage = "Must be either 'O' for One-off or 'M' for Monthly.")]
	public char Period { get; set; }
}
