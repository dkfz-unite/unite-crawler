using Unite.Reader.Models;
using Unite.Reader.Readers;

namespace Unite.Reader.Dna.Sm.Vcf;

public class MetaReader
{
    // ../<project>/omics/<analysis>/<donor>/<sample>/<type>[-vs_<sample>][-<format>]/<file>
    public static Analysis Read(string path)
    {
        var file = new FileInfo(path);
        var typeDirectory = file.Directory;

        return new Analysis
        {
            TargetSample = GetTargetSample(typeDirectory),
            MatchedSample = GetMatchedSample(typeDirectory)
        };
    }

    public static Sample GetTargetSample(DirectoryInfo typeDirectory)
    {
        var sampleDirectory = typeDirectory.Parent;
        var sampleKey = sampleDirectory.Name;

        var metaSample = SampleMetaReader.ReadFromMetaFile(sampleDirectory);
        if (metaSample != null)
            return metaSample;

        var donorDirectory = sampleDirectory.Parent;
        var donorKey = donorDirectory.Name;

        var analysisDirectory = donorDirectory.Parent;

        var analysisSample = SampleMetaReader.ReadFromSamplesFile(analysisDirectory, donorKey, sampleKey);
        if (analysisSample != null)
            return analysisSample;

        var omicsDirectory = analysisDirectory.Parent;
        var projectSample = SampleMetaReader.ReadFromSamplesFile(omicsDirectory, donorKey, sampleKey);
        if (projectSample != null)
            return projectSample;

        return new Sample
        {
            DonorId = donorKey,
            SpecimenId = sampleKey,
            SpecimenType = SpecimenType.Material,
            AnalysisType = AnalysisType.Parse(analysisDirectory.Name),
            AnalysisDate = DateOnly.FromDateTime(analysisDirectory.CreationTimeUtc),
            AnalysisDay = null,
            Genome = null
        };
    }

    public static Sample GetMatchedSample(DirectoryInfo typeDirectory)
    {
        var sampleKey = GetMatchedSampleKey(typeDirectory);
        if (sampleKey == null)
            return null;

        var donorDirectory = typeDirectory.Parent.Parent;
        var donorKey = donorDirectory?.Name;
        if (donorDirectory == null)
            return null;

        var sampleDirectory = donorDirectory.EnumerateDirectories(sampleKey, SearchOption.TopDirectoryOnly).FirstOrDefault();
        if (sampleDirectory != null)
        {
            var sample = SampleMetaReader.ReadFromMetaFile(sampleDirectory);
            if (sample != null)
                return sample;
        }

        var analysisDirectory = donorDirectory.Parent;
        var analysisType = analysisDirectory?.Name;
        if (analysisDirectory == null)
            return null;

        var analysisSample = SampleMetaReader.ReadFromSamplesFile(analysisDirectory, donorKey, sampleKey);
        if (analysisSample != null)
            return analysisSample;

        var omicsDirectory = analysisDirectory.Parent;
        if (omicsDirectory == null)
            return null;

        var projectSamples = SampleMetaReader.ReadFromSamplesFile(omicsDirectory, donorKey, sampleKey);
        if (projectSamples != null)
            return projectSamples;

        return new Sample
        {
            DonorId = donorKey,
            SpecimenId = sampleKey,
            SpecimenType = SpecimenType.Material,
            AnalysisType = AnalysisType.Parse(analysisType),
            AnalysisDate = DateOnly.FromDateTime(analysisDirectory.CreationTimeUtc),
            AnalysisDay = null,
            Genome = null
        };
    }

    private static string GetMatchedSampleKey(DirectoryInfo typeDirectory)
    {
        var parts = typeDirectory.Name.Split('-');
        if (parts.Length < 2)
            return null;

        var sampleKeyPart = parts.FirstOrDefault(part => part.StartsWith("vs_"));
        if (sampleKeyPart == null)
            return null;

        var sampleKeyParts = sampleKeyPart.Split('_');
        if (sampleKeyParts.Length != 2)
            return null;

        return sampleKeyParts[1];
    }
}
