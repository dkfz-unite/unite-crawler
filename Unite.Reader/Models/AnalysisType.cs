namespace Unite.Reader.Models;

public static class AnalysisType
{
    public const string WGS = "WGS";
    public const string WES = "WES";
    public const string MethArray = "MethArray";
    public const string WGBS = "WGBS";
    public const string RRBS = "RRBS";
    public const string RNASeq = "RNASeq";
    public const string RNASeqSc = "scRNASeq";
    public const string RNASeqSn = "snRNASeq";
    public const string ATACSeq = "ATACSeq";
    public const string ATACSeqSc = "scATACSeq";
    public const string ATACSeqSn = "snATACSeq";

    public static readonly string[] All =
    [
        WGS, WES, MethArray, WGBS, RRBS, RNASeq, RNASeqSc, RNASeqSn, ATACSeq, ATACSeqSc, ATACSeqSn
    ];

    public static string Parse(string name)
    {
        var type = All.FirstOrDefault(type => type.Equals(name.Trim(), StringComparison.InvariantCultureIgnoreCase));

        if (type == null)
            throw new ArgumentException($"Unknown analysis type: {name.Trim()}");

        return type;
    }
}
