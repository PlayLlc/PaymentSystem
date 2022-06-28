﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers.Long;
using Play.Ber.Identifiers.Short;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.Tags.__Temp
{
    public partial class TagTests
    {
        #region Instance Members

        [Fact]
        public void Byte_RandomTagNumberLessThan31_IsValidShortIdentifier()
        {
            byte testValue = (byte) _Random.Next(0, ShortIdentifier.TagNumber.MaxValue);
            bool result = ShortIdentifier.IsValid(testValue);

            Assert.True(result);
        }

        [Fact]
        public void Byte_RandomValueWithLongIdentifierFlag_IsNotValidShortIdentifier()
        {
            byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).SetBits(LongIdentifier.LongIdentifierFlag);

            bool result = ShortIdentifier.IsValid(testValue);

            Assert.False(result);
        }

        [Fact]
        public void Byte_TagNumberLessThan31_IsValidShortIdentifier()
        {
            byte testValue = ShortIdentifier.TagNumber.MaxValue;
            bool result = ShortIdentifier.IsValid(testValue);

            Assert.True(result);
        }

        [Fact]
        public void Byte_ValueWithLongIdentifierFlag_IsNotValidShortIdentifier()
        {
            byte testValue = ((byte) _Random.Next(ShortIdentifier.TagNumber.MaxValue + 1, byte.MaxValue)).SetBits(LongIdentifier.LongIdentifierFlag);
            bool result = ShortIdentifier.IsValid(testValue);

            Assert.False(result);
        }

        #endregion
    }
}