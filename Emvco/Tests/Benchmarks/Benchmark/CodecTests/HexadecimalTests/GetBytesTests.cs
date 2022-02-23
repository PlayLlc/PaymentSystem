using System;

using BenchmarkDotNet.Attributes;

using Play.Codecs;

namespace Benchmark.CodecTests.HexadecimalTests;

public class GetBytesTests
{
    #region Static Metadata

    private const int _Iterations = 100_000;

    #endregion

    #region Instance Values

    private readonly HexadecimalCodec _HexadecimalCodec = PlayCodec.HexadecimalCodec;
    private string OneHundred;
    private string OneThousand;
    private string Ten;

    public static ReadOnlySpan<char> CharDictionary =>
        new[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
        };

    #endregion

    #region Instance Members

    [GlobalSetup]
    public void GlobalSetup()
    {
        Random random = new();

        Span<char> ten = stackalloc char[10];
        Span<char> oneHundred = stackalloc char[100];
        Span<char> oneThousand = stackalloc char[1000];

        for (int i = 0; i < 10; i++)
            ten[i] = CharDictionary[random.Next(0, 15)];

        for (int i = 0; i < 100; i++)
            oneHundred[i] = CharDictionary[random.Next(0, 15)];

        for (int i = 0; i < 1000; i++)
            oneThousand[i] = CharDictionary[random.Next(0, 15)];

        Ten = new string(ten);
        OneHundred = new string(oneHundred);
        OneThousand = new string(oneThousand);
    }

    [Benchmark]
    public void GetBytesBenchmark_Ten()
    {
        for (int i = 0; i < _Iterations; i++)
            _HexadecimalCodec.Encode(Ten);
    }

    [Benchmark]
    public void GetBytesBenchmark_OneHundred()
    {
        for (int i = 0; i < _Iterations; i++)
            _HexadecimalCodec.Encode(OneHundred);
    }

    [Benchmark]
    public void GetBytesBenchmark_OneThousand()
    {
        for (int i = 0; i < _Iterations; i++)
            _HexadecimalCodec.Encode(OneThousand);
    }

    #endregion
}