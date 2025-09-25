namespace PTPMQL2526.Models.Program;

public static class AutoCodeGenerator
{
    public static string GenerateNextCode(string id)
    {
        if (string.IsNullOrEmpty(id))
            return "STD001";

        string prefix = new string(id.TakeWhile(c => !char.IsDigit(c)).ToArray());
        string numberPart = new string(id.SkipWhile(c => !char.IsDigit(c)).ToArray());

        int number = int.Parse(numberPart);
        number++;

        string newNumberPart = number.ToString(new string('0', numberPart.Length));

        return prefix + newNumberPart;
    }
}