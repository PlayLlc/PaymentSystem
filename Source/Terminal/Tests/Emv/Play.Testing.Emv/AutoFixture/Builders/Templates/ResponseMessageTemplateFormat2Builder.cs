using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

namespace Play.Testing.Emv.AutoFixture.Builders.Templates;

public class ResponseMessageTemplateFormat2Builder : ConstructedValueSpecimenBuilder<ResponseMessageTemplateFormat2>
{
    public static readonly SpecimenBuilderId Id = new(nameof(ResponseMessageTemplateFormat2));
    private static readonly ResponseMessageTemplateFormat2TestTlv _DefaultTestTlv = new ResponseMessageTemplateFormat2TestTlv();
    private static readonly EmvCodec _Codec = EmvCodec.GetCodec();

    public ResponseMessageTemplateFormat2Builder()
        : base(new DefaultConstructedValueSpecimen<ResponseMessageTemplateFormat2>(ResponseMessageTemplateFormat2.Decode(_Codec, _DefaultTestTlv.EncodeValue()), _DefaultTestTlv.EncodeValue())) { }

    public ResponseMessageTemplateFormat2Builder(DefaultConstructedValueSpecimen<ResponseMessageTemplateFormat2> value) : base(value)
    {
    }

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ResponseMessageTemplateFormat2))
            return new NoSpecimen();

        return GetDefault();
    }

    public override SpecimenBuilderId GetId() => Id;
}
