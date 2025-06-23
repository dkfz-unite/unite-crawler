using Unite.Crawler.Models;

namespace Unite.Crawler.Crawlers;

// ../<project>/omics/<analysis>/<donor>/<sample>/<file>
public abstract class SamplesExplorer : FilesExplorer
{
    protected record Analysis(string Type, string Reader, string Pattern);

    protected abstract Analysis[] Analyses { get; }


    protected FileMetadata[] Explore(string path)
    {
        var files = new List<FileMetadata>();

        if (!Directory.Exists(path))
            return [];

        var rootDirectory = new DirectoryInfo(path);

        var omicsDirectory = rootDirectory.EnumerateDirectories("omics", SearchOptions).FirstOrDefault();
        if (omicsDirectory == null)
            return [];

        foreach (var analysis in Analyses)
        {
            var analysisDirectory = omicsDirectory.EnumerateDirectories($"{analysis.Type}", SearchOptions).FirstOrDefault();
            if (analysisDirectory == null)
                continue;

            foreach (var donorDirectory in analysisDirectory.EnumerateDirectories())
            {
                foreach (var sampleDirectory in donorDirectory.EnumerateDirectories())
                {
                    // Handle OR case
                    if (analysis.Pattern.Contains('|'))
                    {
                        var patterns = analysis.Pattern.Split('|', StringSplitOptions.TrimEntries);

                        foreach (var pattern in patterns)
                        {
                            var file = sampleDirectory.EnumerateFiles(pattern, SearchOptions).FirstOrDefault();
                            if (file == null)
                                continue;

                            files.Add(GetMetadata(file, analysis.Reader));
                            break;
                        }
                    }
                    // Handle OR Else case
                    else if (analysis.Pattern.Contains(','))
                    {
                        var patterns = analysis.Pattern.Split(',', StringSplitOptions.TrimEntries);

                        foreach (var pattern in patterns)
                        {
                            var file = sampleDirectory.EnumerateFiles(pattern, SearchOptions).FirstOrDefault();
                            if (file == null)
                                continue;

                            files.Add(GetMetadata(file, analysis.Reader));
                            continue;
                        }
                    }
                    // Handle AND case
                    else if (analysis.Pattern.Contains('&'))
                    {
                        var patterns = analysis.Pattern.Split('&', StringSplitOptions.TrimEntries);

                        foreach (var pattern in patterns)
                        {
                            var file = sampleDirectory.EnumerateFiles(pattern, SearchOptions).FirstOrDefault();
                            if (file == null)
                                break;

                            files.Add(GetMetadata(file, analysis.Reader));
                            continue;
                        }
                    }
                    // Handle single file case
                    else
                    {
                        var file = sampleDirectory.EnumerateFiles(analysis.Pattern, SearchOptions).FirstOrDefault();
                        if (file == null)
                            continue;

                        files.Add(GetMetadata(file, analysis.Reader));
                    }
                }
            }
        }

        return files.ToArray();
    }
}
