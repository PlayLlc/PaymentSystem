﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;

using Xunit;

namespace Play.Ber.Tests.Tags.__Temp
{
    public partial class TagTests
    {
        #region Instance Members

        [Fact]
        public void ByteArray_WithPrivateConstructedLeadingOctet_CreatesTagWithDataObject()
        {
            ClassTypes expectedClass = ClassTypes.Private;
            DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

            byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
            byte[] testValue = new byte[] {leadingOctet, 45};

            Tag sut = new(testValue);

            Assert.Equal(sut.GetDataObject(), dataObjectType);
        }

        #endregion
    }
}