namespace MvcBank.Utilities;

public static class MiscellaneousExtensionUtilities
{
    // Checks if number has more than a given number of decimal places
    public static bool MoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;
}
