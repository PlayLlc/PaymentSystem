using BenchmarkDotNet.Attributes;

namespace Play.Codecs.Tests.Benchmarks.Hexadecimal;

public partial class HexadecimalBenchmarks
{
    #region Instance Members

    [Benchmark]
    public void HexadecimalCodec_Encoding10_Benchmark()
    {
        for (int i = 0; i < _Data.Iterations._10; i++)
            _HexadecimalCodec.Encode(_Data.Chars._10);
    }

    [Benchmark]
    public void HexadecimalCodec_Encoding100_Benchmark()
    {
        for (int i = 0; i < _Data.Iterations._100; i++)
            _HexadecimalCodec.Encode(_Data.Chars._100);
    }

    [Benchmark]
    public void HexadecimalCodec_Encoding1000_Benchmark()
    {
        for (int i = 0; i < _Data.Iterations._1000; i++)
            _HexadecimalCodec.Encode(_Data.Chars._1000);
    }

    [Benchmark]
    public void HexadecimalCodec_Encoding10000_Benchmark()
    {
        for (int i = 0; i < _Data.Iterations._10000; i++)
            _HexadecimalCodec.Encode(_Data.Chars._10000);
    }

    [Benchmark]
    public void HexadecimalCodec_Encoding100000_Benchmark()
    {
        for (int i = 0; i < _Data.Iterations._100000; i++)
            _HexadecimalCodec.Encode(_Data.Chars._100000);
    }

    #endregion
}