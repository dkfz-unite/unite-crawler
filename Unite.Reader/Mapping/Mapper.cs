using Unite.Essentials.Tsv;
using Unite.Essentials.Tsv.Attributes;
using Unite.Reader.Extensions;
using Unite.Reader.Models;

namespace Unite.Reader.Mapping;

public class Mapper
{
    private readonly ValueMap[] _values = [];

    private Mapper(string path)
    {
        var absolutePath = path.AbsolutePath();

        if (File.Exists(absolutePath))
        {
            var tsv = File.ReadAllText(absolutePath);

            _values = TsvReader.Read<ValueMap>(tsv).ToArray();
        }
    }

    public static Mapper Create(string path)
    {
        var absolutePath = path.AbsolutePath();

        if (File.Exists(absolutePath))
            return new Mapper(absolutePath);
        else
            return null;
    }


    public bool Map(Sample sample, bool allowNull = false)
    {
        if (sample != null)
        {
            var value = Find(sample.DonorId, sample.SpecimenId);

            if (value != null)
            {
                sample.DonorId = value.DonorId ?? sample.DonorId;
                sample.SpecimenId = value.SampleId ?? sample.SpecimenId;
                sample.SpecimenType = value.SampleType ?? sample.SpecimenType;

                return true;
            }
            
            return false;
        }
        
        return allowNull;
    }

    private ValueMap Find(string donorKey, string sampleKey)
    {
        return _values.FirstOrDefault(value => value.DonorKey == donorKey && value.SampleKey == sampleKey)
            ?? _values.FirstOrDefault(value => value.DonorKey == donorKey && value.SampleKey == null)
            ?? _values.FirstOrDefault(value => value.DonorKey == null && value.SampleKey == sampleKey);
    }
}

public class ValueMap
{
    private string _donorKey;
    private string _sampleKey;
    private string _donorId;
    private string _sampleId;
    private string _sampleType;


    [Column("donor_key")]
    public string DonorKey { get => GetString(_donorKey); set => _donorKey = value; }

    [Column("sample_key")]
    public string SampleKey { get => GetString(_sampleKey); set => _sampleKey = value; }

    [Column("donor_id")]
    public string DonorId { get => GetString(_donorId); set => _donorId = value; }

    [Column("sample_id")]
    public string SampleId { get => GetString(_sampleId); set => _sampleId = value; }

    [Column("sample_type")]
    public string SampleType { get => GetType(_sampleType); set => _sampleType = value; }


    private static string GetString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Trim();
    }

    private static string GetType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var type = value?.Trim();

        var comparison = StringComparison.InvariantCultureIgnoreCase;

        if (SpecimenType.Material.Equals(type, comparison))
            return SpecimenType.Material;
        else if (SpecimenType.Line.Equals(type, comparison))
            return SpecimenType.Line;
        else if (SpecimenType.Organoid.Equals(type, comparison))
            return SpecimenType.Organoid;
        else if (SpecimenType.Xenograft.Equals(type, comparison))
            return SpecimenType.Xenograft;
        else
            return SpecimenType.Material;
    }
}
