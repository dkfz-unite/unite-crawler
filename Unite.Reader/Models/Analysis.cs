namespace Unite.Reader.Models;

public class Analysis
{
    public Sample TargetSample;
    public Sample MatchedSample;


    public IEnumerable<string> ToComments()
    {
        var comments = new List<string>();

        if (TargetSample != null)
            comments.AddRange(TargetSample.ToComments("tsample_"));

        if (MatchedSample != null)
            comments.AddRange(MatchedSample.ToComments("msample_"));

        return comments;
    }
}
