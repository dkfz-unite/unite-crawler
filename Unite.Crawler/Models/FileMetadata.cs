using Unite.Essentials.Tsv.Attributes;

namespace Unite.Crawler.Models;

public class FileMetadata
{
    /// <summary>
    /// File name.
    /// </summary>
    [Column("name")]
    public string Name { get; set; }

    /// <summary>
    /// Type of the reader to read the file content (cmd/sm, cmd/cnv, cmd/sv, cmd/exp, etc.).
    /// </summary>
    [Column("reader")] 
    public string Reader { get; set; }

    /// <summary>
    /// Format of the file (tsv, csv, vcf, bam, mex, etc.).
    /// </summary>
    [Column("format")]
    public string Format { get; set; }

    /// <summary>
    /// Archive type (zip, tar, gz, etc.).
    /// </summary>
    [Column("archive")]
    public string Archive { get; set; }

    /// <summary>
    /// Absolute path to the file or folder with the data.
    /// </summary>
    [Column("path")]
    public string Path { get; set; }
}
