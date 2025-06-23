namespace Unite.Crawler.Crawlers.Rna;

public class ExpressionsExplorer : ResultsExplorer
{
    protected override string[] Analyses => ["RNASeq"];
    protected override Calling[] Callings =>
    [
        new Calling("exp", "cmd/rna-exp", "levels.tsv")
    ];
}
