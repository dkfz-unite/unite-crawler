namespace Unite.Crawler.Crawlers.Dna;

public class SamplesExplorer : Crawlers.SamplesExplorer
{
    protected override Analysis[] Analyses =>
    [
        new ("WGS", "cmd/meta", "*.fasta*, *.fastq*, *.bam, *.bam.bai, *.bam.bai.md5"),
        new ("WES", "cmd/meta", "*.fasta*, *.fastq*, *.bam, *.bam.bai, *.bam.bai.md5")
    ];
}
