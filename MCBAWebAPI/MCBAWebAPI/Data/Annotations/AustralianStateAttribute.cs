namespace System.ComponentModel.DataAnnotations;

public class AustralianStateAttribute : ValidationAttribute 
{
	public List<string> States { get; set; }

	public AustralianStateAttribute() => States = new() { "VIC", "SA", "WA", "QLD", "TAS", "NT", "NSW", "ACT" };

	public override bool IsValid(object value)
    {
		string strValue = value as string;

		// Check if the value matches any Australian states 
		foreach(string s in States)
        {
			if(strValue == s)
            {
				return true;
            }
			// Modifies error message if lower case state names were given
			else if(strValue.ToUpper() == s)
			{
				ErrorMessage = "Must be in capital letters.";
				return false;
            }
        }

		ErrorMessage = "Must be a valid Australian state. (example: VIC, WA)";
		return false;
    }
}
