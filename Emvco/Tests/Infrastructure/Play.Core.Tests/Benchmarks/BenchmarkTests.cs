using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using Play.Codecs.Tests.Benchmarks.Hexadecimal;
using Play.Core.Tests.Benchmarks.Extensions._Temp;
using Play.Core.Tests.Benchmarks.Extensions._Tempc;

using Xunit;

namespace Play.Codecs.Tests.Benchmarks
{
    public class BenchmarkTests
    {
        #region Static Metadata

        public static readonly BenchmarkTestData Data = new();

        #endregion

        #region Instance Values

        private readonly bool _IsBenchmarkTestingFlagSet;

        #endregion

        #region Constructor

        public BenchmarkTests()
        {
            // HACK: We need to put this in a json value so we can control it globally
            _IsBenchmarkTestingFlagSet = false;
        }

        #endregion

        #region Instance Members

        [SkippableFact]
        public void HexadecimalCodec_HexadecimalBenchmarks_Summary()
        {
            Skip.IfNot(_IsBenchmarkTestingFlagSet);

            WriteSummaryReport(BenchmarkRunner.Run<SpanExtensionBenchmarks>());
            WriteSummaryReport(BenchmarkRunner.Run<UlongExtensionBenchmarks>());
        }

        private void WriteSummaryReport(Summary summary)
        {
            // TODO: Write these summaries somewhere on file or to some cloud storage for analytics on change of rate and monitoring performance deprecation over the time of the modules

            Console.WriteLine(summary.ToString());
        }

        #endregion
    }
}