public static class NumberFormatter
{
    private static readonly string[] Units = new string[] { "", "K", "M", "G", "T" };

    public static string FormatNumber(double number)
    {
        int unit_index = 0;

        while (number >= 1000 && unit_index < Units.Length - 1)
        {
            number /= 1000;
            unit_index++;
        }

        return number.ToString("0.##") + Units[unit_index];
    }
}