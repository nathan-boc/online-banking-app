using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBank.Models;

public class Customer 
{
	[MaxLength(4), MinLength(4)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int CustomerID { get; set; }

	[Required, MaxLength(50)]
	public string Name { get; set; }

	[MaxLength(11)]
	public string TFN { get; set; }

	[MaxLength(50)]
	public string Address { get; set; }

	[MaxLength(40)]
	public string Suburb { get; set; }

	[AustralianState]
	public string State { get; set; }

	// Regex to match postcodes 0000 - 9999
	[RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Must be a valid postcode between 0000 - 9999.")]
	public string Postcode { get; set; }

	// Regex to match phone number format '04XX XXX XXX'
	[RegularExpression(@"^04[0-9]{2} [0-9]{3} [0-9]{3}$", ErrorMessage = "Must be in the format '04XX XXX XXX'.")]
	public string Mobile { get; set; }
}
