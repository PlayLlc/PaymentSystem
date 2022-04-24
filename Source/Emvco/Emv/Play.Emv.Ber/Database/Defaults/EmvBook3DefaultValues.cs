using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber;

public class EmvBook3DefaultValues : DefaultValues
{
    #region Instance Members

    /// <summary>
    ///     Initializes default values that are specified in EMVco Book 3
    /// </summary>
    public override void AddDefaults(KnownObjects knownObjects, Dictionary<Tag, PrimitiveValue> defaultValueMap)
    {
        foreach (PrimitiveValue? prim in GetActionCodeDefaults())
        {
            if (knownObjects.Exists(prim.GetTag()))
                defaultValueMap.TryAdd(prim.GetTag(), prim);
        }
    }

    /// <summary>
    ///     EMVco Book 3 specifies the default values to be used for Terminal and Issuer Action Codes if they're not present in
    ///     the PICC or Terminal
    /// </summary>
    /// <remarks>
    ///     EMVco Book 3 Section 10.7
    /// </remarks>
    private static PrimitiveValue[] GetActionCodeDefaults()
    {
        return new PrimitiveValue[]
        {
            new TerminalActionCodeDenial(0), new TerminalActionCodeDefault(0), new TerminalActionCodeOnline(0),
            new IssuerActionCodeDenial(0), new IssuerActionCodeOnline(ulong.MaxValue), new IssuerActionCodeDefault(ulong.MaxValue)
        };
    }

    #endregion
}