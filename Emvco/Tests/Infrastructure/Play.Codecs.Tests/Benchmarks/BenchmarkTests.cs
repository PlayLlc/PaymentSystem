﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using Play.Codecs.Tests.Benchmarks.Hexadecimal;

using Xunit;

namespace Play.Codecs.Tests.Benchmarks
{
    public class BenchmarkTests
    {
        #region Static Metadata

        public static readonly BenchmarkTestData Data = new();

        #endregion

        #region Instance Members

        [Fact]
        public void HexadecimalCodec_HexadecimalBenchmarks_Summary()
        {
            WriteSummaryReport(BenchmarkRunner.Run<HexadecimalBenchmarks>());
        }

        private void WriteSummaryReport(Summary summary)
        {
            // TODO: Write these summaries somewhere on file or to some cloud storage for analytics on change of rate and monitoring performance deprecation over the time of the modules

            Console.WriteLine(summary.ToString());
        }

        #endregion
    }
}