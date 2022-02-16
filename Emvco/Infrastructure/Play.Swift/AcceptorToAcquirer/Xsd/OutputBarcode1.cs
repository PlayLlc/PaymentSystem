using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.001.001.09")]
public partial class OutputBarcode1
{
    #region Instance Values

    private BarcodeType1Code brcdTpField;
    private string brcdValField;
    private byte[] qRCdBinryValField;
    private string qRCdVrsnField;
    private QRCodeEncodingMode1Code qRCdNcodgMdField;
    private QRCodeErrorCorrection1Code qRCdErrCrrctnField;
    private bool qRCdErrCrrctnFieldSpecified;

    /// <remarks />
    public BarcodeType1Code BrcdTp
    {
        get => this.brcdTpField;
        set => this.brcdTpField = value;
    }

    /// <remarks />
    public string BrcdVal
    {
        get => this.brcdValField;
        set => this.brcdValField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] QRCdBinryVal
    {
        get => this.qRCdBinryValField;
        set => this.qRCdBinryValField = value;
    }

    /// <remarks />
    public string QRCdVrsn
    {
        get => this.qRCdVrsnField;
        set => this.qRCdVrsnField = value;
    }

    /// <remarks />
    public QRCodeEncodingMode1Code QRCdNcodgMd
    {
        get => this.qRCdNcodgMdField;
        set => this.qRCdNcodgMdField = value;
    }

    /// <remarks />
    public QRCodeErrorCorrection1Code QRCdErrCrrctn
    {
        get => this.qRCdErrCrrctnField;
        set => this.qRCdErrCrrctnField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool QRCdErrCrrctnSpecified
    {
        get => this.qRCdErrCrrctnFieldSpecified;
        set => this.qRCdErrCrrctnFieldSpecified = value;
    }

    #endregion
}