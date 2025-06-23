using Unite.Crawler.Models;

namespace Unite.Crawler.Crawlers;

public abstract class FilesExplorer
{
    protected virtual EnumerationOptions SearchOptions => new()
    {
        MatchCasing = MatchCasing.CaseInsensitive,
        RecurseSubdirectories = false
    };

    protected virtual FileMetadata GetMetadata(FileInfo file, string reader)
    {
        return new FileMetadata
        {
            Name = file.Name,
            Reader = reader,
            Format = file.Extension.TrimStart('.').ToLower(),
            Archive = GetArchive(file.Extension),
            Path = file.FullName
        };
    }

    /// <summary>
    /// Get the reader based on the folder name.
    /// </summary>
    /// <param name="name">Folder name (e.g. "cnv-vs_blood-aceseq").</param>
    /// <param name="prefix">Prefix to add to the reader name (e.g. "cmd/dna-cnv").</param>
    /// <returns>Reader with "-{prefix}" or null, if no reader specified in folder name.</returns>
    protected virtual string GetReader(string name, string prefix)
    {
        if (!name.Contains('-'))
            return null;

        var parts = name.Split('-');
        
        if (parts.Length == 2)
            return $"{prefix}-{parts[1]}".ToLower();
        else if (parts.Length == 3)
            return $"{prefix}-{parts[2]}".ToLower();
        else
            return null;
    }

    /// <summary>
    /// Get the archive type based on the file extension.
    /// </summary>
    /// <param name="extension">File extension (e.g. ".gz", ".zip").</param>
    /// <returns>>Archive type as a string ("gz", "zip") or null if not recognized.</returns>
    protected virtual string GetArchive(string extension)
    {
        return extension.EndsWith(".gz") ? "gz" :
               extension.EndsWith(".zip") ? "zip" :
               null;
    }
}
