namespace Unite.Reader.Models;

public class Sample
{
    private string _specimenType;

    public string DonorId;
    public string SpecimenId;
    public string SpecimenType { get => _specimenType ?? Models.SpecimenType.Material; set => _specimenType = value; }
    public string AnalysisType;
    public DateOnly? AnalysisDate;
    public int? AnalysisDay;
    public string Genome;
    public double? Purity;
    public double? Ploidy;
    public int? Cells;


    public List<string> ToComments(string prefix = null)
    {
        var pref = prefix == null ? "" : $"{prefix}";

        var comments = new List<string>();

        if (DonorId != null)
            comments.Add($"{pref}donor_id: {DonorId}");

        if (SpecimenId != null)
            comments.Add($"{pref}specimen_id: {SpecimenId}");

        if (SpecimenType != null)
            comments.Add($"{pref}specimen_type: {SpecimenType}");

        if (AnalysisType != null)
            comments.Add($"{pref}analysis_type: {AnalysisType}");

        if (AnalysisDate != null && AnalysisDate != default)
            comments.Add($"{pref}analysis_date: {AnalysisDate:yyyy-MM-dd}");

        if (AnalysisDay != null)
            comments.Add($"{pref}analysis_day: {AnalysisDay}");

        if (Genome != null)
            comments.Add($"{pref}genome: {Genome}");

        if (Purity != null)
            comments.Add($"{pref}purity: {Purity}");

        if (Ploidy != null)
            comments.Add($"{pref}ploidy: {Ploidy}");

        if (Cells != null)
            comments.Add($"{pref}cells: {Cells}");

        return comments;
    }

    public static Sample FromComments(string[] comments)
    {
        var sample = new Sample();

        foreach (var comment in comments)
        {
            if (TryGetValue("donor_id", comment, out var donorId))
                sample.DonorId = donorId;
            else if (TryGetValue("specimen_id", comment, out var specimenId))
                sample.SpecimenId = specimenId;
            else if (TryGetValue("specimen_type", comment, out var specimenType))
                sample.SpecimenType = specimenType;
            else if (TryGetValue("analysis_type", comment, out var analysisType))
                sample.AnalysisType = analysisType;
            else if (TryGetValue("analysis_date", comment, out var analysisDate))
                sample.AnalysisDate = DateOnly.Parse(analysisDate);
            else if (TryGetValue("analysis_day", comment, out var analysisDay))
                sample.AnalysisDay = int.Parse(analysisDay);
            else if (TryGetValue("genome", comment, out var genome))
                sample.Genome = genome;
            else if (TryGetValue("purity", comment, out var purity))
                sample.Purity = double.Parse(purity);
            else if (TryGetValue("ploidy", comment, out var ploidy))
                sample.Ploidy = double.Parse(ploidy);
            else if (TryGetValue("cells", comment, out var cells))
                sample.Cells = int.Parse(cells);
            else
                sample = null;
        }

        return sample;
    }


    private static bool TryGetValue(string name, string comment, out string value)
    {
        value = null;

        if (!comment.StartsWith($"{name}", StringComparison.InvariantCultureIgnoreCase))
            return false;

        if (!comment.Contains(':'))
            return false;

        var parts = comment.Split(':', 2);
        if (parts.Length != 2)
            return false;

        value = parts[1].Trim();

        return true;
    }
}
