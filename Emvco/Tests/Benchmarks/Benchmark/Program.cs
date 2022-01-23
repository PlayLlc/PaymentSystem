using System;

using Benchmark.CodecTests.HexadecimalTests;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using Play.Codecs;
using Play.Codecs.Strings;

namespace Benchmark;

public class SpanBenchmarks
{
    #region Static Metadata

    private const int Iterations = 1_000;

    #endregion

    #region Instance Values

    private Hexadecimal Hexadecimal = PlayEncoding.Hexadecimal;
    private byte[] OneHundred;
    private byte[] OneThousand;
    private byte[] Ten;

    #endregion

    #region Instance Members

    [GlobalSetup]
    public void GlobalSetup()
    {
        byte[] ten = new byte[10];
        byte[] oneHundred = new byte[50];
        byte[] oneThousand = new byte[10000];

        new Random().NextBytes(ten);
        new Random().NextBytes(oneHundred);
        new Random().NextBytes(oneThousand);

        Ten = ten;
        OneHundred = oneHundred;
        OneThousand = oneThousand;
    }

    #endregion

    //[Benchmark]
    //public void PassSpanByValue()
    //{
    //	for (int i = 0; i < Iterations; i++) AcceptSpanByValue(_data);
    //}

    //[Benchmark]
    //public void PassSpanByRef()
    //{
    //	for (int i = 0; i < Iterations; i++) AcceptSpanByRef(_data);
    //}

    //[Benchmark]
    //public void PassLargeStructByValue()
    //{
    //	for (int i = 0; i < Iterations; i++) AcceptLargeStructByValue(_control);
    //}

    //[Benchmark]
    //public void PassLargeStructByRef()
    //{
    //	for (int i = 0; i < Iterations; i++) AcceptLargeStructByRef(_control);
    //}

    //[Benchmark]
    //public void PassLargeStructByValue2()
    //{
    //	for (int i = 0; i < Iterations; i++) AcceptLargeStructByValue2(_control2);
    //} 

    //[Benchmark]
    //public void GetStringWithStackAllocTen()
    //{
    //	ReadOnlySpan<byte> a = Ten;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAlloc(a);
    //}

    //[Benchmark]
    //public void GetStringWithStackAllocPinnableTen()
    //{
    //	ReadOnlySpan<byte> a = Ten;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAllocPinnable(a);
    //}

    //[Benchmark]
    //public void GetStringWithStackAllocOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneHundred;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAlloc(a);
    //}

    //[Benchmark]
    //public void GetStringWithStackAllocPinnableOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneHundred;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAllocPinnable(a);
    //}

    //[Benchmark]
    //public void GetStringWithStackAllocOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneHundred;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAlloc(a);
    //}

    //[Benchmark]
    //public void GetStringWithStackAllocOneThousand()
    //{
    //	ReadOnlySpan<byte> a = OneThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAlloc(a);
    //}

    //[Benchmark]
    //public void GetStringWithStackAllocTenThousand()
    //{
    //	ReadOnlySpan<byte> a = TenThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAlloc(a);
    //}

    //[Benchmark]
    //public void GetStringWithStackAllocOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithStackAllocPinnable(a);
    //}

    //[Benchmark]
    //public void GetStringWithSpanOwnerPinnableOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithSpanOwnerPinnable(a);
    //}

    //[Benchmark]
    //public void GetStringWithSpanOwnerOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneHundred;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithSpanOwner(a);
    //}

    //[Benchmark]
    //public void GetStringWithSpanOwnerOneThousand()
    //{
    //	ReadOnlySpan<byte> a = OneThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithSpanOwner(a);
    //}

    //[Benchmark]
    //public void GetStringWithSpanOwnerTenThousand()
    //{
    //	ReadOnlySpan<byte> a = TenThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringWithSpanOwner(a);
    //}

    //#region SpanOwner And GetChar

    //[Benchmark]
    //public void GetStringWithSpanOwnerAndCharTen()
    //{
    //	ReadOnlySpan<byte> a = Ten;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    //}

    //[Benchmark]
    //public void GetStringWithSpanOwnerAndCharOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneHundred;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    //}

    //[Benchmark]
    //public void GetStringWithSpanOwnerAndCharOneThousand()
    //{
    //	ReadOnlySpan<byte> a = OneThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    //}

    ////[Benchmark]
    ////public void GetStringWithSpanOwnerAndCharTenThousand()
    ////{
    ////	ReadOnlySpan<byte> a = TenThousand;
    ////	for (int i = 0; i < Iterations; i++)
    ////		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    ////}

    //#endregion

    //#region Stackalloc And GetChar

    //[Benchmark]
    //public void GetStringByStackallocAndCharTen()
    //{
    //	ReadOnlySpan<byte> a = Ten;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    //}

    //[Benchmark]
    //public void GetStringByStackallocAndCharOneHundred()
    //{
    //	ReadOnlySpan<byte> a = OneHundred;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    //}

    //[Benchmark]
    //public void GetStringByStackallocAndCharOneThousand()
    //{
    //	ReadOnlySpan<byte> a = OneThousand;
    //	for (int i = 0; i < Iterations; i++)
    //		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    //}

    ////[Benchmark]
    ////public void GetStringByStackallocAndCharTenThousand()
    ////{
    ////	ReadOnlySpan<byte> a = TenThousand;
    ////	for (int i = 0; i < Iterations; i++)
    ////		Hexadecimal.GetStringBySpanOwnerAndChar(a);
    ////}

    //#endregion
}

public class Program
{
    #region Instance Members

    public static void Main(string[] args)
    {
        Summary summary = BenchmarkRunner.Run<GetBytesTests>();
    }

    #endregion
}