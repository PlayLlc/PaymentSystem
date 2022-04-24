using Play.Emv.Ber.Enums;

namespace Play.Emv.Icc;

public class GetFileControlInformationRApduSignal : RApduSignal
{
    #region Constructor

    public GetFileControlInformationRApduSignal(ReadOnlySpan<byte> response) : base(response.ToArray())
    { }

    public GetFileControlInformationRApduSignal(ReadOnlySpan<byte> response, Level1Error level1Error) : base(response.ToArray(), level1Error)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    public Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}