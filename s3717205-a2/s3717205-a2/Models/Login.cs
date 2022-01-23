﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBank.Models;

public class Login 
{
	[StringLength(8, MinimumLength = 8)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public string LoginID { get; set; }

	public int CustomerID { get; set; }
	public virtual Customer Customer { get; set; }

	[Required, StringLength(64)]
	public string PasswordHash { get; set; }
}
