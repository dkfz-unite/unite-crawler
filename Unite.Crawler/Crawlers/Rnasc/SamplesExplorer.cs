namespace Unite.Crawler.Crawlers.Rnasc;

public class SamplesExplorer : Crawlers.SamplesExplorer
{
    protected override Analysis[] Analyses =>
    [
        new ("scRNASeq", "cmd/meta", "*.fasta*, *.fastq*, *.bam, *.bam.bai, *.bam.bai.md5"),
        new ("snRNASeq", "cmd/meta", "*.fasta*, *.fastq*, *.bam, *.bam.bai, *.bam.bai.md5")
    ];
}
