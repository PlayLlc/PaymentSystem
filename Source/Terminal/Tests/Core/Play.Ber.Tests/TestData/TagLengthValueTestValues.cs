using System.Collections.Generic;

namespace Play.Ber.Tests.TestData;

internal class TagLengthValueTestValues
{
    #region Static Metadata

    private static readonly List<TagLengthValueTestValue> _ValidTestValues;

    #endregion

    #region Constructor

    static TagLengthValueTestValues()
    {
        _ValidTestValues = new List<TagLengthValueTestValue>
        {
            new(new byte[] {0x9F, 0x40}, new byte[] {0x05}, new byte[] {0xFF, 0x80, 0xF0, 0xF0, 0x01}),
            new(new byte[] {0x9F, 0x02}, new byte[] {0x06}, new byte[] {0x00, 0x00, 0x00, 0x01, 0x23, 0x45}),
            new(new byte[] {0x9F, 0x03}, new byte[] {0x06}, new byte[] {0x00, 0x00, 0x00, 0x00, 0x40, 0x00}),
            new(new byte[] {0x9F, 0x26}, new byte[] {0x08}, new byte[] {0x8E, 0x19, 0xED, 0x4B, 0xCA, 0x5C, 0x67, 0x0A}),
            new(new byte[] {0x82}, new byte[] {0x02}, new byte[] {0x5C, 0x00}),
            new(new byte[] {0x5F, 0x34}, new byte[] {0x01}, new byte[] {0x02}),
            new(new byte[] {0x5F, 0x34}, new byte[] {0x01}, new byte[] {0x02})
        };
    }

    #endregion

    #region Instance Members

    public static IEnumerable<object[]> GetValidTestValues()
    {
        for (int i = 0; i < _ValidTestValues.Count; i++)
            yield return new object[] {_ValidTestValues[i]};
    }

    #endregion
}