using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using Play.Codecs.Tests.Alphabetic;

using Xunit;

namespace Play.Codecs.Tests.Benchmarks.Hexadecimal
{
    public partial class HexadecimalBenchmarks
    {
        #region Instance Values

        private readonly BenchmarkTestData _Data = BenchmarkTests.Data;
        private readonly HexadecimalCodec _HexadecimalCodec = PlayCodec.HexadecimalCodec;

        #endregion
    }
}