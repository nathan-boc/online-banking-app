using System.ComponentModel.DataAnnotations;

namespace MvcBank.Models;

public class Payee
{
	public int PayeeID { get; set; }

	[Required, StringLength(50)]
	public string Name { get; set; }

	[Required, StringLength(50)]
	public string Address { get; set; }

	[Required, StringLength(40)]
	public string Suburb { get; set; }

	[Required, AustralianState]
	public string State { get; set; }

	// Regex to match postcodes 0000 - 9999
	[Required, RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Must be a valid postcode between 0000 - 9999.")]
	public string Postcode { get; set; }

	// Regex to match phone number format '(0X) XXXX XXXX'
	[Required, RegularExpression(@"^(0[0-9]{1}) [0-9]{4} [0-9]{4}$", ErrorMessage = "Must be in the format '(0X) XXXX XXXX'.")]
	public string Phone { get; set; }

	public virtual List<BillPay> BillPays { get; set; }
}
