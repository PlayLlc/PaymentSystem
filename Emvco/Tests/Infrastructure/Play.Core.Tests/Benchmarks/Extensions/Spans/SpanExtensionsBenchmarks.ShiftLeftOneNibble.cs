using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;

namespace Play.Core.Tests.Benchmarks.Extensions._Temp
{
    public partial class SpanExtensionBenchmarks
    {
        #region Instance Members

        /// <summary>
        ///     GetStringBenchmark_Ten
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void SpanExtension_ShiftLeftOneNibble10_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._10; i++)
                _Data.Bytes._10.AsSpan().ShiftLeftOneNibble();
        }

        /// <summary>
        ///     GetStringBenchmark_OneHundred
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void SpanExtension_ShiftLeftOneNibble100_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._100; i++)
                _Data.Bytes._10.AsSpan().ShiftLeftOneNibble();
        }

        /// <summary>
        ///     GetStringBenchmark_OneThousand
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void SpanExtension_ShiftLeftOneNibble1000_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._1000; i++)
                _Data.Bytes._10.AsSpan().ShiftLeftOneNibble();
        }

        /// <summary>
        ///     GetStringBenchmark_TenThousand
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void SpanExtension_ShiftLeftOneNibble10000_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._10000; i++)
                _Data.Bytes._10.AsSpan().ShiftLeftOneNibble();
        }

        /// <summary>
        ///     GetStringBenchmark_TenThousand
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void SpanExtension_ShiftLeftOneNibble100000_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._100000; i++)
                _Data.Bytes._10.AsSpan().ShiftLeftOneNibble();
        }

        #endregion
    }
}