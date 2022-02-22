using Play.Codecs;
using Play.Core.Extensions;

namespace Play.Randoms;

public partial class Randomize
{
    public class Integers
    {
        #region Instance Members

        public static sbyte SByte() => (sbyte) _Random.Next(sbyte.MinValue, sbyte.MaxValue);

        public static sbyte SByte(byte min, byte max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            return (sbyte) _Random.Next(min, max);
        }

        public static byte Byte() => (byte) _Random.Next(byte.MinValue, byte.MaxValue);

        public static byte Byte(byte min, byte max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            return (byte) _Random.Next(min, max);
        }

        public static short Short() => (short) _Random.Next(short.MinValue, short.MaxValue);

        public static short Short(short min, short max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            return (short) _Random.Next(min, max);
        }

        public static ushort UShort() => (ushort) _Random.Next(ushort.MinValue, ushort.MaxValue);

        public static ushort UShort(ushort min, ushort max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            return (ushort) _Random.Next(min, max);
        }

        public static int Int() => _Random.Next(int.MinValue, int.MaxValue);

        public static int Int(int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            return _Random.Next(0, max - min);
        }

        public static uint UInt()
        {
            unchecked
            {
                uint result = (uint) _Random.Next(ushort.MinValue, int.MaxValue);
                result += (uint) _Random.Next(ushort.MinValue, int.MaxValue);

                return result;
            }
        }

        public static uint UInt(uint min, uint max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            uint result = 0;

            if (min > int.MaxValue)
            {
                result += int.MaxValue;
                result += (uint) _Random.Next(0, (int) (max - min));

                return result;
            }

            if (max > int.MaxValue)
            {
                result += (uint) _Random.Next(0, int.MaxValue);
                int maxCeiling = (int) ((max - result) > int.MaxValue ? int.MaxValue : max - result);
                result += (uint) _Random.Next(0, maxCeiling);

                return result;
            }

            return (uint) _Random.Next((int) min, (int) max);
        }

        public static long Long()
        {
            unchecked
            {
                long result = (long) _Random.Next(int.MinValue, int.MaxValue) << 32;
                result += _Random.Next(int.MinValue, int.MaxValue);

                return result;
            }
        }

        public static long Long(long min, long max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min),
                    $"The argument {nameof(min)} must be less than the argument {nameof(max)}");
            }

            if (min == max)
                return min;

            return GetRandomFromHash(min, max, Long());
        }

        public static ulong ULong()
        {
            Span<byte> randNumberBuffer = stackalloc byte[8];
            _Random.NextBytes(randNumberBuffer);

            return PlayEncoding.UnsignedIntegerCodec.GetUInt64(randNumberBuffer);
        }

        private static long GetRandomFromHash(long min, long max, long hash)
        {
            (MinMax MinMax, long Distance) distanceData = GetLongDistanceData(min, max, hash);
            ulong absoluteDistance = (ulong) Math.Abs(distanceData.Distance);
            ulong randomNumberMask = absoluteDistance.GetMaskAfterMostSignificantBit();

            Span<byte> randNumberBuffer = stackalloc byte[absoluteDistance.GetMostSignificantByte()];
            _Random.NextBytes(randNumberBuffer);
            long correction = (long) PlayEncoding.UnsignedIntegerCodec.GetUInt64(randNumberBuffer).GetMaskedValue(randomNumberMask);

            return distanceData.MinMax == MinMax.Min ? hash - correction : hash + correction;
        }

        private static (MinMax MinMax, long Distance) GetLongDistanceData(long min, long max, long hash)
        {
            long distanceFromMin = min - hash;
            long distanceFromMax = max - hash;

            if (Math.Abs(distanceFromMin) > Math.Abs(distanceFromMax))
                return (MinMax.Min, distanceFromMin);

            return (MinMax.Max, distanceFromMin);
        }

        #endregion

        private enum MinMax : byte
        {
            Min,
            Max
        }
    }
}