using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBank.Models;

public class Account
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	[MaxLength(4), MinLength(4)]
	public int AccountNumber { get; set; }

	[Required]
	[RegularExpression("^[CS]{1}", ErrorMessage = "Must be either 'C' for Checking or 'S' for Savings.")]
	public char AccountType { get; set; }

	public int CustomerID { get; set; }
	public virtual Customer Customer { get; set; }

	[Required]
	[DataType(DataType.Currency)]
	public decimal Balance { get; set; }

	[InverseProperty(nameof(Transaction.Account))]
	public virtual List<Transaction> Transactions { get; set; }

	public virtual List<BillPay> BillPays { get; set; }
}
