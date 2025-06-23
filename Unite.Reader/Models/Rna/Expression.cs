using Unite.Essentials.Tsv.Attributes;

namespace Unite.Reader.Models.Rna;

public class Expression
{
    [Column("gene_id")]
    public string GeneId { get; set; }
    [Column("exonic_length")]
    public int ExonicLength { get; set; }
    [Column("reads")]
    public int Reads { get; set; }
}
