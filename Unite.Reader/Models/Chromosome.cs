namespace Unite.Reader.Models;

public static class ChromosomeType
{
    public const string Chr1 = "1";
    public const string Chr2 = "2";
    public const string Chr3 = "3";
    public const string Chr4 = "4";
    public const string Chr5 = "5";
    public const string Chr6 = "6";
    public const string Chr7 = "7";
    public const string Chr8 = "8";
    public const string Chr9 = "9";
    public const string Chr10 = "10";
    public const string Chr11 = "11";
    public const string Chr12 = "12";
    public const string Chr13 = "13";
    public const string Chr14 = "14";
    public const string Chr15 = "15";
    public const string Chr16 = "16";
    public const string Chr17 = "17";
    public const string Chr18 = "18";
    public const string Chr19 = "19";
    public const string Chr20 = "20";
    public const string Chr21 = "21";
    public const string Chr22 = "22";
    public const string ChrX = "X";
    public const string ChrY = "Y";
    public const string ChrMT = "MT";

    public static readonly string[] All =
    [
        Chr1, Chr2, Chr3, Chr4, Chr5, Chr6, Chr7, Chr8, Chr9, Chr10,
        Chr11, Chr12, Chr13, Chr14, Chr15, Chr16, Chr17, Chr18, Chr19,
        Chr20, Chr21, Chr22, ChrX, ChrY, ChrMT
    ];

    public static string Parse(string name)
    {
        var type = All.FirstOrDefault(type => type.Equals(name.Trim(), StringComparison.InvariantCultureIgnoreCase));

        if (type == null)
            throw new ArgumentException($"Unknown chromosome type: {name.Trim()}");

        return type;
    }
}
