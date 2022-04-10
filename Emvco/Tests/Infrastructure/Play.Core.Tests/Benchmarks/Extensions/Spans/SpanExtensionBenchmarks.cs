using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Play.Codecs;
using Play.Codecs.Tests.Benchmarks;
using Play.Codecs.Tests.Benchmarks.Hexadecimal;
using Play.Core.Extensions;

namespace Play.Core.Tests.Benchmarks.Extensions._Temp
{
    public partial class SpanExtensionBenchmarks
    {
        #region Instance Values

        private readonly BenchmarkTestData _Data = BenchmarkTests.Data;

        #endregion
    }
}