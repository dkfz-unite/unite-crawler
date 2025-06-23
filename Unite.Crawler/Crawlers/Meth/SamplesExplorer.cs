namespace Unite.Crawler.Crawlers.Meth;

public class SamplesExplorer : Crawlers.SamplesExplorer
{
    protected override Analysis[] Analyses =>
    [
        new ("MethArray", "cmd/meta", "*red.idat & *grn.idat"),
        new ("WGBS", "cmd/meta", "*.fasta*, *.fastq*, *.bam, *.bam.bai, *.bam.bai.md5"),
        new ("RRBS", "cmd/meta", "*.fasta*, *.fastq*, *.bam, *.bam.bai, *.bam.bai.md5")
    ]; 
}
