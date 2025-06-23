using Unite.Essentials.Tsv.Attributes;

namespace Unite.Reader.Models.Dna;

public class Sv
{
    [Column("chromosome_1")]
    public string Chromosome1 { get; set; }
    [Column("start_1")]
    public int? Start1 { get; set; }
    [Column("end_1")]
    public int? End1 { get; set; }
    [Column("flanking_sequence_1")]
    public string FlankingSequence1 { get; set; }
    [Column("chromosome_2")]
    public string Chromosome2 { get; set; }
    [Column("start_2")]
    public int? Start2 { get; set; }
    [Column("end_2")]
    public int? End2 { get; set; }
    [Column("flanking_sequence_2")]
    public string FlankingSequence2 { get; set; }
    [Column("type")]
    public string Type { get; set; }
    [Column("inverted")]
    public bool? Inverted { get; set; }
}
