using Unite.Reader.Extensions;

if (args.Length != 1)
{
    Console.Error.WriteLine($"Requires 1 argument: <path>.");
    return;
}

var path = args[0].AbsolutePath();
if (!File.Exists(path))
{
    Console.Error.WriteLine($"File '{path}' not found.");
    return;
}

var sample = Unite.Reader.Readers.SampleMetaReader.Read(path);

var txt = string.Join(Environment.NewLine, sample.ToComments("#"));

Console.WriteLine(txt);
