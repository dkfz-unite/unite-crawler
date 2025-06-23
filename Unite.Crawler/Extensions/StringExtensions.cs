namespace Unite.Crawler.Extensions;

public static class StringExtensions
{
    public static string AbsolutePath(this string path)
    {
        if (path.StartsWith('/'))
        {
            return path;
        }
        else
        {
            var relativePath = Path.GetRelativePath(Environment.CurrentDirectory, path);
            var absolutePath = Path.GetFullPath(relativePath);
            return absolutePath;
        }
    }
}
