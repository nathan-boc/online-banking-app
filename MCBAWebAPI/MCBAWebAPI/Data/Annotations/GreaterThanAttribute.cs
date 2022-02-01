namespace System.ComponentModel.DataAnnotations;

public class GreaterThanAttribute : ValidationAttribute 
{
	public double Minimum { get; set; }

	public GreaterThanAttribute(double minimum) => Minimum = minimum;

	public override bool IsValid(object value)
    {
		double dblValue = (double) value;

		if (dblValue > Minimum) {
			return true;
        }

		ErrorMessage = $"Must be greater than {Minimum}";
		return false;
    }
}
