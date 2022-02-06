using Benchmark.CodecTests.HexadecimalTests;

using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Benchmark;

public class Program
{
    #region Instance Members

    public static void Main(string[] args)
    {
        Summary summary = BenchmarkRunner.Run<GetBytesTests>();
    }

    #endregion
}