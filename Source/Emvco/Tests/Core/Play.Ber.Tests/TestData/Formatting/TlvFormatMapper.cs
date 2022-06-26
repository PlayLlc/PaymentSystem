//using System.Collections.Generic;
//using System.Linq;

//using Play.Ber.DataObjects;
//using Play.Core.Iso8825.Ber;
//using Play.Core.Iso8825.Ber.Contents;

//namespace Play.Core.Iso8825.Tests.Ber.TestData.Formatting
//{
//    public class TlvFormatMapper : ITlvFormatMapper
//    {
//        #region Instance Values

//        public Dictionary<int, PrimitiveValue> TlvFormatMappings =>
//            Enumerable.Range(0, short.MaxValue).Select(a => new KeyValuePair<int, PrimitiveValue>(a, HexadecimalPrimitive.Default))
//                .ToDictionary(b => b.Key, b => b.Value);

//        #endregion
//    }
//}

