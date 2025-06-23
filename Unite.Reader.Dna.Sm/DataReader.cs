using Unite.Reader.Models;

namespace Unite.Reader.Dna.Sm;

/// <summary>
/// Reads SM files in VCF format.
/// </summary>
public static class DataReader
{
    public static IEnumerable<Variant> Read(string path)
    {
        using var reader = new StreamReader(path);

        while(!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (line.StartsWith('#'))
            {
                continue;
            }

            if (TryParse(line, out var variant))
            {
                yield return variant;
            }
        }
    }

    private static bool TryParse(string line, out Variant variant)
    {
        var fields = line.Split('\t');

        if (ChromosomeType.All.Contains(fields[0], StringComparer.InvariantCultureIgnoreCase))
        {
            variant = new Variant
            {
                Chromosome = fields[0],
                Position = fields[1],
                Ref = fields[3],
                Alt = fields[4]
            };

            return true;
        }
        else
        {
            variant = null;

            return false;
        }
    }
}
