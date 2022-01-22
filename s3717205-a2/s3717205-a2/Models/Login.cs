using System.ComponentModel.DataAnnotations;

namespace MvcBank.Models;

public class Login 
{
	[Required]
	[MaxLength(8), MinLength(8)]
	public string LoginID { get; set; }

	[Required]
	// TODO : Add Customer foreign key annotation here
	public int CustomerID { get; set; }

	[Required]
	[MaxLength(64)]
	public string PasswordHash { get; set; }
}
