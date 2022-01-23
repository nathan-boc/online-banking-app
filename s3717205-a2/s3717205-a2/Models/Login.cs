using System.ComponentModel.DataAnnotations;

namespace MvcBank.Models;

public class Login 
{
	[MaxLength(8), MinLength(8)]
	public string LoginID { get; set; }

	public int CustomerID { get; set; }
	public virtual Customer Customer { get; set; }

	[Required, MaxLength(64)]
	public string PasswordHash { get; set; }
}
