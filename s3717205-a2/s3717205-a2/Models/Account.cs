using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBank.Models;

public class Account
{
	[StringLength(4, MinimumLength = 4)]
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int AccountNumber { get; set; }

	[Required]
	[RegularExpression("^[CS]{1}", ErrorMessage = "Must be either 'C' for Checking or 'S' for Savings.")]
	public char AccountType { get; set; }

	public int CustomerID { get; set; }
	public virtual Customer Customer { get; set; }

	[Required]
	[DataType(DataType.Currency), Column(TypeName = "money")]
	public decimal Balance { get; set; }

	// Specifies 'Account' property to be the property the join is performed on (rather than DestinationAccount)
	[InverseProperty(nameof(Transaction.Account))]
	public virtual List<Transaction> Transactions { get; set; }

	public virtual List<BillPay> BillPays { get; set; }
}
