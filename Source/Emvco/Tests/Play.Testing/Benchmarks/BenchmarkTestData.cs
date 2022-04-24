using Play.Randoms;

namespace Play.Testing.Benchmarks;

public class BenchmarkTestData
{
    #region Instance Values

    public readonly ByteArrayBuffers Bytes = new();
    public readonly CharArrayBuffers Chars = new();
    public readonly UlongBuffers Ulong = new();
    public readonly TestIterations Iterations = new();

    #endregion

    public class TestIterations
    {
        #region Instance Values

        public int _10 = 10;
        public int _100 = 100;
        public int _1000 = 1000;
        public int _10000 = 10000;
        public int _100000 = 100000;

        #endregion
    }

    public class ByteArrayBuffers
    {
        #region Instance Values

        public readonly byte[] _10 = Randomize.Hex.Bytes(10);
        public readonly byte[] _100 = Randomize.Hex.Bytes(100);
        public readonly byte[] _1000 = Randomize.Hex.Bytes(1000);
        public readonly byte[] _10000 = Randomize.Hex.Bytes(10000);
        public readonly byte[] _100000 = Randomize.Hex.Bytes(100000);

        #endregion
    }

    public class UlongBuffers
    {
        #region Instance Values

        public readonly ulong[] _10 = GetUlongData(10);
        public readonly ulong[] _100 = GetUlongData(100);
        public readonly ulong[] _1000 = GetUlongData(1000);
        public readonly ulong[] _10000 = GetUlongData(10000);
        public readonly ulong[] _100000 = GetUlongData(100000);

        #endregion

        #region Instance Members

        private static ulong[] GetUlongData(int count)
        {
            ulong[] result = new ulong[count];
            for (int i = 0; i < count; i++)
                result[i] = Randomize.Integers.ULong();

            return result;
        }

        #endregion
    }

    public class SpanByteBuffers
    {
        #region Instance Values

        public readonly byte[] _10 = Randomize.Hex.Bytes(10);
        public readonly byte[] _100 = Randomize.Hex.Bytes(100);
        public readonly byte[] _1000 = Randomize.Hex.Bytes(1000);
        public readonly byte[] _10000 = Randomize.Hex.Bytes(10000);
        public readonly byte[] _100000 = Randomize.Hex.Bytes(100000);

        #endregion
    }

    public class CharArrayBuffers
    {
        #region Instance Values

        public readonly char[] _10 = Randomize.Hex.Chars(10);
        public readonly char[] _100 = Randomize.Hex.Chars(100);
        public readonly char[] _1000 = Randomize.Hex.Chars(1000);
        public readonly char[] _10000 = Randomize.Hex.Chars(10000);
        public readonly char[] _100000 = Randomize.Hex.Chars(100000);

        #endregion
    }
}