namespace Unite.Crawler.Crawlers.Dna;

public class VariantsExplorer: ResultsExplorer
{
    protected override string[] Analyses => ["WGS", "WES"];
    protected override Calling[] Callings =>
    [
        new ("snv", "cmd/dna-sm", "variants.tsv | variants.vcf"),
        new ("indel", "cmd/dna-sm", "variants.tsv | variants.vcf"),
        new ("sm", "cmd/dna-sm", "variants.tsv | variants.vcf"),
        new ("cnv", "cmd/dna-cnv", "variants-*-*.tsv | variants-*-*.vcf | variants.tsv | variants.vcf"),
        new ("sv", "cmd/dna-sv", "variants.tsv | variants.vcf")
    ];
}
