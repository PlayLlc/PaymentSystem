﻿using System;
using System.Collections.Generic;

using Play.Core.Specifications;
using Play.Randoms;

namespace Play.Core.Tests.Data.Fixtures;

internal class IntFixture
{
    public class MostSignificantBit
    {
        #region Static Metadata

        private static readonly Random _Random = new();

        #endregion

        #region Instance Members

        public static IEnumerable<object[]> ForByte(int count)
        {
            for (int i = 0; i < count; i++)
            {
                byte value = Randomize.Integers.Byte(byte.MinValue, byte.MaxValue);

                yield return new object[] {GetMostSignificantBit(value), value};
            }
        }

        public static IEnumerable<object[]> ForUShort(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ushort value = Randomize.Integers.UShort(byte.MinValue, byte.MaxValue);

                yield return new object[] {GetMostSignificantBit(value), value};
            }
        }

        public static IEnumerable<object[]> ForUInt(int count)
        {
            for (int i = 0; i < count; i++)
            {
                uint value = Randomize.Integers.UShort(byte.MinValue, byte.MaxValue);

                yield return new object[] {GetMostSignificantBit(value), value};
            }
        }

        public static IEnumerable<object[]> ForULong(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ulong value = Randomize.Integers.UShort(byte.MinValue, byte.MaxValue);

                yield return new object[] {GetMostSignificantBit(value), value};
            }
        }

        public static IEnumerable<object[]> ForPositiveShort(int count)
        {
            for (int i = 0; i < count; i++)
            {
                short value = Randomize.Integers.Short(0, short.MaxValue);

                yield return new object[] {GetMostSignificantBit(value), value};
            }
        }

        public static IEnumerable<object[]> ForNegativeShort(int count)
        {
            for (int i = 0; i < count; i++)
            {
                short value = Randomize.Integers.Short(short.MinValue, 0);

                yield return new object[] {value};
            }
        }

        private static int GetMostSignificantBit(byte value)
        {
            if (value == 0)
                return 0;

            byte buffer = value;
            int offset = 0;

            for (int i = 0; i < Specs.Integer.Int8.BitCount; i++)
            {
                if (buffer == 0)
                    return offset;

                if ((buffer % 10) != 0)
                    offset = i + 1;

                buffer >>= 1;
            }

            return offset;
        }

        private static int GetMostSignificantBit(ushort value)
        {
            if (value == 0)
                return 0;

            ushort buffer = value;
            int offset = 0;

            for (int i = 0; i < Specs.Integer.Int16.BitCount; i++)
            {
                if (buffer == 0)
                    return offset;

                if ((buffer % 10) != 0)
                    offset = i + 1;

                buffer >>= 1;
            }

            return offset;
        }

        private static int GetMostSignificantBit(short value)
        {
            if (value == 0)
                return 0;

            short buffer = value;
            int offset = 0;

            for (int i = 0; i < Specs.Integer.Int16.BitCount; i++)
            {
                if (buffer == 0)
                    return offset;

                if ((buffer % 10) != 0)
                    offset = i + 1;

                buffer >>= 1;
            }

            return offset;
        }

        private static int GetMostSignificantBit(uint value)
        {
            if (value == 0)
                return 0;

            uint buffer = value;
            int offset = 0;

            for (int i = 0; i < Specs.Integer.Int32.BitCount; i++)
            {
                if (buffer == 0)
                    return offset;

                if ((buffer % 10) != 0)
                    offset = i + 1;

                buffer >>= 1;
            }

            return offset;
        }

        private static int GetMostSignificantBit(ulong value)
        {
            if (value == 0)
                return 0;

            ulong buffer = value;
            int offset = 0;

            for (int i = 0; i < Specs.Integer.Int64.BitCount; i++)
            {
                if (buffer == 0)
                    return offset;

                if ((buffer % 10) != 0)
                    offset = i + 1;

                buffer >>= 1;
            }

            return offset;
        }

        #endregion

        public record MostSignificantByteTestData
        {
            #region Instance Values

            public byte TestData { get; set; }
            public int MostSignificantBit { get; set; }

            #endregion
        }
    }
}