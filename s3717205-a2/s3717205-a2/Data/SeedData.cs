using MvcBank.Models;

namespace MvcBank.Data;

public static class SeedData
{
	public static void Initialise(IServiceProvider serviceProvider)
	{
		var context = serviceProvider.GetRequiredService<MvcBankContext>();

		// Checks if any data exists
		if (context.Customer.Any())
			return;

		else
			context.Customer.AddRange(
				// Add customer data here
				);

			context.SaveChanges();
	}
}