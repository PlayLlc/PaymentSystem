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
public partial class NetworkParameters7
{
    #region Instance Values

    private NetworkParameters9[] adrField;
    private string usrNmField;
    private byte[] accsCdField;
    private byte[][] svrCertField;
    private byte[][] svrCertIdrField;
    private byte[][] clntCertField;
    private string sctyPrflField;

    /// <remarks />
    [XmlElement("Adr")]
    public NetworkParameters9[] Adr
    {
        get => this.adrField;
        set => this.adrField = value;
    }

    /// <remarks />
    public string UsrNm
    {
        get => this.usrNmField;
        set => this.usrNmField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] AccsCd
    {
        get => this.accsCdField;
        set => this.accsCdField = value;
    }

    /// <remarks />
    [XmlElement("SvrCert", DataType = "base64Binary")]
    public byte[][] SvrCert
    {
        get => this.svrCertField;
        set => this.svrCertField = value;
    }

    /// <remarks />
    [XmlElement("SvrCertIdr", DataType = "base64Binary")]
    public byte[][] SvrCertIdr
    {
        get => this.svrCertIdrField;
        set => this.svrCertIdrField = value;
    }

    /// <remarks />
    [XmlElement("ClntCert", DataType = "base64Binary")]
    public byte[][] ClntCert
    {
        get => this.clntCertField;
        set => this.clntCertField = value;
    }

    /// <remarks />
    public string SctyPrfl
    {
        get => this.sctyPrflField;
        set => this.sctyPrflField = value;
    }

    #endregion
}