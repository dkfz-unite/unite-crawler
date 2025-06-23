using Unite.Essentials.Tsv.Attributes;

namespace Unite.Reader.Models.Dna;

public class Cnv
{
    [Column("chromosome")]
    public string Chromosome { get; set; }
    [Column("start")]
    public int Start { get; set; }
    [Column("end")]
    public int End { get; set; }
    [Column("type")]
    public string Type { get; set; }
    [Column("loh")]
    public bool? Loh { get; set; }
    [Column("del")]
    public bool? Del { get; set; }
    [Column("c1_mean")]
    public double? C1Mean { get; set; }
    [Column("c2_mean")]
    public double? C2Mean { get; set; }
    [Column("tcn_mean")]
    public double? TcnMean { get; set; }
    [Column("c1")]
    public int? C1 { get; set; }
    [Column("c2")]
    public int? C2 { get; set; }
    [Column("tcn")]
    public int? Tcn { get; set; }
    [Column("dh_max")]
    public double? DhMax { get; set; }
}
