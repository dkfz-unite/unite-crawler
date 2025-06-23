namespace Unite.Crawler.Crawlers.Rna;

public class SamplesExplorer : Crawlers.SamplesExplorer
{
    protected override Analysis[] Analyses =>
    [
        new ("RNASeq", "cmd/meta", "*.fasta*, *.fastq*, *.bam, *.bam.bai, *.bam.bai.md5")
    ];
}
