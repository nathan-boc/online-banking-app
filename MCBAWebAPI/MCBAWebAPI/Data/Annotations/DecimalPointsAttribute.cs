namespace System.ComponentModel.DataAnnotations;

public class DecimalPointsAttribute : ValidationAttribute 
{
	public int Decimals { get; set; }

	public DecimalPointsAttribute(int decimals) => Decimals = decimals;

	public override bool IsValid(object value)
    {
		decimal decValue = (decimal) value;

		if(decValue == Math.Round(decValue, Decimals)) {
			return true;
        }

		ErrorMessage = $"Must contain {Decimals} decimal places.";
		return false;
    }
}
