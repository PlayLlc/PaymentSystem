using BenchmarkDotNet.Attributes;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;

namespace Play.Core.Tests.Benchmarks.Extensions.Ulong;

public partial class UlongExtensionBenchmarks
{
    #region Instance Members

    /// <summary>
    ///     GetStringBenchmark_Ten
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Benchmark]
    public void UlongExtension_GetMaskedValue10_Benchmark()
    {
        for (int i = 0; i < (_Data.Iterations._10 - 1); i++)
            _Data.Ulong._10[i].GetMaskedValue(_Data.Ulong._10[i + 1]);
    }

    /// <summary>
    ///     GetStringBenchmark_OneHundred
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Benchmark]
    public void UlongExtension_GetMaskedValue100_Benchmark()
    {
        for (int i = 0; i < (_Data.Iterations._100 - 1); i++)
            _Data.Ulong._100[i].GetMaskedValue(_Data.Ulong._100[i + 1]);
    }

    /// <summary>
    ///     GetStringBenchmark_OneThousand
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Benchmark]
    public void UlongExtension_GetMaskedValue1000_Benchmark()
    {
        for (int i = 0; i < (_Data.Iterations._1000 - 1); i++)
            _Data.Ulong._1000[i].GetMaskedValue(_Data.Ulong._1000[i + 1]);
    }

    /// <summary>
    ///     GetStringBenchmark_TenThousand
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Benchmark]
    public void UlongExtension_GetMaskedValue10000_Benchmark()
    {
        for (int i = 0; i < (_Data.Iterations._10000 - 1); i++)
            _Data.Ulong._10000[i].GetMaskedValue(_Data.Ulong._10000[i + 1]);
    }

    /// <summary>
    ///     GetStringBenchmark_TenThousand
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Benchmark]
    public void UlongExtension_GetMaskedValue100000_Benchmark()
    {
        for (int i = 0; i < (_Data.Iterations._100000 - 1); i++)
            _Data.Ulong._100000[i].GetMaskedValue(_Data.Ulong._100000[i + 1]);
    }

    #endregion
}