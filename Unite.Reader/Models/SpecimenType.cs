namespace Unite.Reader.Models;

public static class SpecimenType
{
    public const string Material = "Material";
    public const string Line = "Line";
    public const string Organoid = "Organoid";
    public const string Xenograft = "Xenograft";

    public static readonly string[] All =
    [
        Material, Line, Organoid, Xenograft
    ];

    public static string Parse(string name)
    {
        var type = All.FirstOrDefault(type => type.Equals(name.Trim(), StringComparison.InvariantCultureIgnoreCase));

        if (type == null)
            throw new ArgumentException($"Unknown specimen type: {name.Trim()}");

        return type;
    }
}
