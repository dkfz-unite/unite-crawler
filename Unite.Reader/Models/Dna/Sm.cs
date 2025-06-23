using Unite.Essentials.Tsv.Attributes;

namespace Unite.Reader.Models.Dna;

public class Sm
{
    [Column("chromosome")]
    public string Chromosome { get; set; }
    [Column("position")]
    public string Position { get; set; }
    [Column("ref")]
    public string Ref { get; set; }
    [Column("alt")]
    public string Alt { get; set; }
}
