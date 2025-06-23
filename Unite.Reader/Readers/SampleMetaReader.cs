using Unite.Essentials.Tsv;
using Unite.Reader.Models;

namespace Unite.Reader.Readers;

// ../<project>/omics/samples.tsv - all samples metadata.
// ../<project>/omics/<analysis>/samples.tsv - all samples metadata for a specific analysis type.
// ../<project>/omics/<analysis>/<donor>/<sample>/meta.tsv - current sample metadata.
// ../<project>/omics/<analysis>/<donor>/<sample>/<file>
public class SampleMetaReader
{
    private const StringComparison _comparison = StringComparison.InvariantCultureIgnoreCase;

    public static Sample Read(string path)
    {
        var file = new FileInfo(path);

        var metaSample = ReadFromMetaFile(file.Directory);
        if (metaSample != null)
            return metaSample;

        var sheetSample = ReadFromSamplesFile(path);
        if (sheetSample != null)
            return sheetSample;

        
        var blocks = path.Split('/');

        var sampleKey = blocks[^2];
        var donorKey = blocks[^3];
        var analysisType = blocks[^4];

        return new Sample
        {
            DonorId = donorKey,
            SpecimenId = sampleKey,
            SpecimenType = SpecimenType.Material,
            AnalysisType = AnalysisType.Parse(analysisType),
            AnalysisDate = DateOnly.FromDateTime(file.CreationTime),
            AnalysisDay = null,
            Genome = null
        };
    }

    public static Sample ReadFromMetaFile(DirectoryInfo sampleDirectory)
    {
        var metaFile = sampleDirectory.EnumerateFiles("meta.tsv").FirstOrDefault();
        if (metaFile == null)
            return null;

        using var reader = new StreamReader(metaFile.FullName);

        var lines = new List<string>();

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line != null && line.StartsWith('#'))
                lines.Add(line.TrimStart('#', ' ').Trim());
            else
                break; // Stop reading at the first non-comment line.
        }

        return Sample.FromComments(lines.ToArray());
    }

    public static Sample ReadFromSamplesFile(DirectoryInfo directory, string donorKey, string sampleKey)
    {
        var file = directory.EnumerateFiles("samples.tsv", SearchOption.TopDirectoryOnly).FirstOrDefault();
        if (file == null)
            return null;

        return Find(file.FullName, donorKey, sampleKey);
    }

    private static Sample ReadFromSamplesFile(string path)
    {
        var sampleFile = new FileInfo(path);
        var sampleDirectory = sampleFile.Directory;
        var sampleKey = sampleDirectory.Name;

        var donorDirectory = sampleDirectory.Parent;
        var donorKey = donorDirectory?.Name;
        if (donorKey == null)
            return null;

        var analysisDirectory = donorDirectory.Parent;
        var analysisType = analysisDirectory?.Name;
        if (analysisType == null)
            return null;

        var analysisSample = ReadFromSamplesFile(analysisDirectory, donorKey, sampleKey);
        if (analysisSample != null)
            return analysisSample;

        var omicsDirectory = analysisDirectory.Parent;
        if (omicsDirectory == null)
            return null;

        var projectSamples = ReadFromSamplesFile(omicsDirectory, donorKey, sampleKey);
        if (projectSamples != null)
            return projectSamples;

        return null;
    }

    private static Sample Find(string filePath, string donorKey, string sampleKey)
    {
        try
        {
            using var reader = new StreamReader(filePath);

            var meta = TsvReader.Read<SampleMeta>(reader).FirstOrDefault(entry =>
                entry.DonorKey.Trim().Equals(donorKey, _comparison) &&
                entry.SampleKey.Trim().Equals(sampleKey, _comparison)
            );

            if (meta == null)
                return null;

            return new Sample
            {
                DonorId = meta.DonorId ?? meta.DonorKey,
                SpecimenId = meta.SpecimenId ?? meta.SampleKey,
                SpecimenType = SpecimenType.Parse(meta.SpecimenType),
                AnalysisType = AnalysisType.Parse(meta.AnalysisType),
                AnalysisDate = meta.AnalysisDate,
                AnalysisDay = meta.AnalysisDay,
                Genome = meta.Genome
            };
        }
        catch
        {
            throw new Exception($"Failed to read sample metadata from '{filePath}'.");
        }
    }
}
