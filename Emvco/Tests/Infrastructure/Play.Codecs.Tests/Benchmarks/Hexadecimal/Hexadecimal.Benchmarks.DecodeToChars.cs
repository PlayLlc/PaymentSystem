using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Play.Codecs.Exceptions;

namespace Play.Codecs.Tests.Benchmarks.Hexadecimal
{
    public partial class HexadecimalBenchmarks
    {
        #region Instance Members

        /// <summary>
        ///     GetStringBenchmark_Ten
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void HexadecimalCodec_DecodeToString10_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._10; i++)
                _HexadecimalCodec.DecodeToString(_Data.Bytes._10);
        }

        /// <summary>
        ///     GetStringBenchmark_OneHundred
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void HexadecimalCodec_DecodeToString100_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._100; i++)
                _HexadecimalCodec.DecodeToString(_Data.Bytes._100);
        }

        /// <summary>
        ///     GetStringBenchmark_OneThousand
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void HexadecimalCodec_DecodeToString1000_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._1000; i++)
                _HexadecimalCodec.DecodeToString(_Data.Bytes._1000);
        }

        /// <summary>
        ///     GetStringBenchmark_TenThousand
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void HexadecimalCodec_DecodeToString10000_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._10000; i++)
                _HexadecimalCodec.DecodeToString(_Data.Bytes._10000);
        }

        /// <summary>
        ///     GetStringBenchmark_TenThousand
        /// </summary>
        /// <exception cref="CodecParsingException"></exception>
        [Benchmark]
        public void HexadecimalCodec_DecodeToString100000_Benchmark()
        {
            for (int i = 0; i < _Data.Iterations._100000; i++)
                _HexadecimalCodec.DecodeToString(_Data.Bytes._100000);
        }

        #endregion
    }
}