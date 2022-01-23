using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBank.Models;

public class Customer 
{
	[StringLength(4, MinimumLength = 4)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int CustomerID { get; set; }

	[Required, StringLength(50)]
	public string Name { get; set; }

	[StringLength(11)]
	public string TFN { get; set; }

	[StringLength(50)]
	public string Address { get; set; }

	[StringLength(40)]
	public string Suburb { get; set; }

	[AustralianState]
	public string State { get; set; }

	// Regex to match postcodes 0000 - 9999
	[RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Must be a valid postcode between 0000 - 9999.")]
	public string Postcode { get; set; }

	// Regex to match phone number format '04XX XXX XXX'
	[RegularExpression(@"^04[0-9]{2} [0-9]{3} [0-9]{3}$", ErrorMessage = "Must be in the format '04XX XXX XXX'.")]
	public string Mobile { get; set; }

	public virtual Login Login { get; set; }

	public virtual List<Account> Accounts { get; set; }
}
