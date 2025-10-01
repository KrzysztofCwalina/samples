
Dictionary<byte[], byte[]> dictionary = new(BufferComparer.Instance);
dictionary.Set("foo", "bar"u8);
if (dictionary.ContainsKey("foo"u8))
{
    Console.WriteLine("found!");
}

static class BufferDictionaryExtensions
{
    public static bool ContainsKey(this Dictionary<byte[], byte[]> dict, ReadOnlySpan<byte> key)
    {
        var lookup = dict.GetAlternateLookup<ReadOnlySpan<byte>>();
        return lookup.ContainsKey(key);
    }
    public static void Set(this Dictionary<byte[], byte[]> dict, ReadOnlySpan<byte> key, ReadOnlySpan<byte> value)
    {
        var lookup = dict.GetAlternateLookup<ReadOnlySpan<byte>>();
        lookup[key] = value.ToArray(); 
    }
}

class BufferComparer : IEqualityComparer<byte[]>, IAlternateEqualityComparer<ReadOnlySpan<byte>, byte[]>
{
    public static BufferComparer Instance { get; } = new BufferComparer();
    public bool Equals(byte[] x, byte[] y) => x.AsSpan().SequenceEqual(y.AsSpan());

    public int GetHashCode(byte[] obj) => GetHashCode(obj.AsSpan());
    
    // IAlternateEqualityComparer<ReadOnlySpan<byte>, byte[]> implementation
    public bool Equals(ReadOnlySpan<byte> alternate, byte[]? other) => alternate.SequenceEqual(other.AsSpan());
    
    public int GetHashCode(ReadOnlySpan<byte> alternate)
    {
        HashCode hash = new();
        foreach (byte b in alternate) {
            hash.Add(b);
        }
        return hash.ToHashCode();
    }

    public byte[] Create(ReadOnlySpan<byte> alternate) =>alternate.ToArray();
}