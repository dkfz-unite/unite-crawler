using Unite.Essentials.Tsv.Attributes;

namespace Unite.Reader.Models;

public class SampleMeta
{
    [Column("donor_key")]
    public string DonorKey { get; set; }

    [Column("sample_id")]
    public string SampleKey { get; set; }

    [Column("donor_id")]
    public string DonorId { get; set; }

    [Column("specimen_id")]
    public string SpecimenId { get; set; }

    [Column("specimen_type")]
    public string SpecimenType { get; set; }

    [Column("analysis_type")]
    public string AnalysisType { get; set; }

    [Column("analysis_date")]
    public DateOnly? AnalysisDate { get; set; }

    [Column("analysis_day")]
    public int? AnalysisDay { get; set; }

    [Column("genome")]
    public string Genome { get; set; }
}
