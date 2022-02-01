using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

using MvcBank.Models;

namespace MvcBank.Data;

public static class SeedData
{
	public static void Initialise(IServiceProvider serviceProvider, string url)
	{
		var context = serviceProvider.GetRequiredService<MvcBankContext>();

		// Checks if any data exists
		if (context.Customer.Any())
			return;

		else
			context.Customer.AddRange(DeserialiseJson(url));
			context.SaveChanges();
	}

	private static List<Customer> DeserialiseJson(string url)
    {
		string jsonData = new HttpClient().GetStringAsync(url).Result;

		List<Customer> users = null;

		try
		{
			// Deserialize JSON data into list of Customer objects
			users = JsonConvert.DeserializeObject<List<Customer>>(jsonData, new JsonSerializerSettings
			{
				DateFormatString = "dd/MM/yyyy"
			});

			decimal currentBalance = 0;

			// All transactions from JSON set as deposit type
			foreach (Customer customer in users)
				foreach (Account account in customer.Accounts)
				{
					currentBalance = 0;

					foreach (Transaction transaction in account.Transactions)
					{
						// Code assumes all seeded transactions are deposits
						transaction.TransactionType = 'D';
						currentBalance += transaction.Amount;
					}

					account.Balance = currentBalance;
				}
		
		}
		catch(InvalidDataException ex)
		{
			Console.WriteLine("Error: " + ex.ToString());
		}

		return users;
	}
}