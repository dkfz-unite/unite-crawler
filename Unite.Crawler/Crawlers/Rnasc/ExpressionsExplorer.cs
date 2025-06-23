namespace Unite.Crawler.Crawlers.Rnasc;

public class ExpressionsExplorer : ResultsExplorer
{
    protected override string[] Analyses => ["scRNASeq", "snRNASeq"];

    protected override Calling[] Callings =>
    [
        new ("exp", "cmd/rnasc-exp", "features.tsv.gz & barcodes.tsv.gz & matrix.mtx.gz")
    ];
}
