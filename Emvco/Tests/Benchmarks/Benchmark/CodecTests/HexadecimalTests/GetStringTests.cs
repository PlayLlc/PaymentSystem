using System;

using BenchmarkDotNet.Attributes;

using Play.Codecs;

namespace Benchmark.CodecTests.HexadecimalTests;

public class GetStringTests
{
    #region Static Metadata

    private const int _Iterations = 100_000;

    #endregion

    #region Instance Values

    private readonly HexadecimalCodec _HexadecimalCodec = PlayCodec.HexadecimalCodec;
    private byte[] OneHundred;
    private byte[] OneThousand;
    private byte[] Ten;
    private byte[] TenThousand;

    #endregion

    #region Instance Members

    [GlobalSetup]
    public void GlobalSetup()
    {
        Random random = new();
        byte[] ten = new byte[10];
        byte[] oneHundred = new byte[100];
        byte[] oneThousand = new byte[1000];
        byte[] tenThousand = new byte[10000];

        random.NextBytes(ten);
        random.NextBytes(oneHundred);
        random.NextBytes(oneThousand);
        random.NextBytes(tenThousand);

        Ten = ten;
        OneHundred = oneHundred;
        OneThousand = oneThousand;
        TenThousand = tenThousand;
    }

    /// <summary>
    /// GetStringBenchmark_Ten
    /// </summary>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    [Benchmark]
    public void GetStringBenchmark_Ten()
    {
        ReadOnlySpan<byte> oneThousand = Ten;
        for (int i = 0; i < _Iterations; i++)
            _HexadecimalCodec.DecodeToString(oneThousand);
    }

    /// <summary>
    /// GetStringBenchmark_OneHundred
    /// </summary>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    [Benchmark]
    public void GetStringBenchmark_OneHundred()
    {
        ReadOnlySpan<byte> oneThousand = OneHundred;
        for (int i = 0; i < _Iterations; i++)
            _HexadecimalCodec.DecodeToString(oneThousand);
    }

    /// <summary>
    /// GetStringBenchmark_OneThousand
    /// </summary>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    [Benchmark]
    public void GetStringBenchmark_OneThousand()
    {
        ReadOnlySpan<byte> oneThousand = OneThousand;
        for (int i = 0; i < _Iterations; i++)
            _HexadecimalCodec.DecodeToString(oneThousand);
    }

    /// <summary>
    /// GetStringBenchmark_TenThousand
    /// </summary>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    [Benchmark]
    public void GetStringBenchmark_TenThousand()
    {
        ReadOnlySpan<byte> oneThousand = TenThousand;
        for (int i = 0; i < _Iterations; i++)
            _HexadecimalCodec.DecodeToString(oneThousand);
    }

    #endregion
}