using AutoFixture.Kernel;

using Play.Emv.Ber.Templates;
using Play.Testing.Icc.Apdu;

namespace Play.Testing.Emv.Contactless.AutoFixture;

//public class FileControlInformationAdfBuilder : SpecimenBuilder
//{
//    #region Static Metadata

//    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationAdfBuilder));

//    #endregion

//    #region Instance Members

//    public override SpecimenBuilderId GetId() => Id;

//    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
//    public override object Create(object request, ISpecimenContext context)
//    {
//        Type? type = request as Type;

//        if (type == null)
//            return new NoSpecimen();

//        if (type != typeof(FileControlInformationAdf))
//            return new NoSpecimen();

//        return FileControlInformationAdf.Decode(ApduTestData.RApdu.Select.Applet1.Bytes);
//    }

//    #endregion
//}