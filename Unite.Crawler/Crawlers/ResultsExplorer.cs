using Unite.Crawler.Models;

namespace Unite.Crawler.Crawlers;

// ../<project>/omics/<analysis>/<donor>/<sample>/<type>[-vs_<sample>][-<format>]/<file>
public abstract class ResultsExplorer : FilesExplorer
{
    protected record Calling(string Type, string Reader, string Pattern);

    protected abstract string[] Analyses { get; }
    protected abstract Calling[] Callings { get; }


    protected virtual FileMetadata[] Explore(string path)
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
            var analysisDirectory = omicsDirectory.EnumerateDirectories($"{analysis}", SearchOptions).FirstOrDefault();
            if (analysisDirectory == null)
                continue;

            foreach (var donorDirectory in analysisDirectory.EnumerateDirectories())
            {
                foreach (var sampleDirectory in donorDirectory.EnumerateDirectories())
                {
                    foreach (var calling in Callings)
                    {
                        var typeDirectory = sampleDirectory.EnumerateDirectories($"{calling.Type}*", SearchOptions).FirstOrDefault();
                        if (typeDirectory == null)
                            continue;

                        var reader = GetReader(typeDirectory.Name, calling.Reader) ?? calling.Reader;

                        // Handle OR case
                        if (calling.Pattern.Contains('|'))
                        {
                            var patterns = calling.Pattern.Split('|', StringSplitOptions.TrimEntries);

                            foreach (var pattern in patterns)
                            {
                                var file = sampleDirectory.EnumerateFiles(pattern, SearchOptions).FirstOrDefault();
                                if (file == null)
                                    continue;

                                files.Add(GetMetadata(file, calling.Reader));
                                break;
                            }
                        }
                        // Handle OR Else case
                        else if (calling.Pattern.Contains(','))
                        {
                            var patterns = calling.Pattern.Split(',', StringSplitOptions.TrimEntries);

                            foreach (var pattern in patterns)
                            {
                                var file = sampleDirectory.EnumerateFiles(pattern, SearchOptions).FirstOrDefault();
                                if (file == null)
                                    continue;

                                files.Add(GetMetadata(file, calling.Reader));
                                continue;
                            }
                        }
                        // Handle AND case
                        else if (calling.Pattern.Contains('&'))
                        {
                            var patterns = calling.Pattern.Split('&', StringSplitOptions.TrimEntries);

                            foreach (var pattern in patterns)
                            {
                                var file = sampleDirectory.EnumerateFiles(pattern, SearchOptions).FirstOrDefault();
                                if (file == null)
                                    break;

                                files.Add(GetMetadata(file, calling.Reader));
                                continue;
                            }
                        }
                        // Handle single file case
                        else
                        {
                            var file = sampleDirectory.EnumerateFiles(calling.Pattern, SearchOptions).FirstOrDefault();
                            if (file == null)
                                continue;

                            files.Add(GetMetadata(file, calling.Reader));
                        }
                    }
                }
            }
        }

        return files.ToArray();
    }
}
