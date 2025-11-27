namespace PTPMQL2526.Models.Program;

public static class AutoCodeGenerator
{
    public static string GenerateNextCode(string lastID, string prefix)
    {
        if (string.IsNullOrEmpty(lastID))
        {
            return prefix + "001";
        }
        if (!lastID.StartsWith(prefix))
        {
            return prefix + "001";
        }

        string numberPart = new string(lastID.SkipWhile(c => !char.IsDigit(c)).ToArray());

        int number = int.Parse(numberPart);
        number++;

        string newNumberPart = number.ToString(new string('0', numberPart.Length));

        return prefix + newNumberPart;
    }
}