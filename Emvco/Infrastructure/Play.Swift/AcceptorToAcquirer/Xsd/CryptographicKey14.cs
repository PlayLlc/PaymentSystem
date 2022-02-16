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
public partial class CryptographicKey14
{
    #region Instance Values

    private string idField;
    private byte[] addtlIdField;
    private string nmField;
    private string sctyPrflField;
    private string itmNbField;
    private string vrsnField;
    private CryptographicKeyType3Code tpField;
    private bool tpFieldSpecified;
    private KeyUsage1Code[] fctnField;
    private DateTime actvtnDtField;
    private bool actvtnDtFieldSpecified;
    private DateTime deactvtnDtField;
    private bool deactvtnDtFieldSpecified;
    private ContentInformationType23 keyValField;
    private byte[] keyChckValField;
    private GenericInformation1[] addtlMgmtInfField;

    /// <remarks />
    public string Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] AddtlId
    {
        get => this.addtlIdField;
        set => this.addtlIdField = value;
    }

    /// <remarks />
    public string Nm
    {
        get => this.nmField;
        set => this.nmField = value;
    }

    /// <remarks />
    public string SctyPrfl
    {
        get => this.sctyPrflField;
        set => this.sctyPrflField = value;
    }

    /// <remarks />
    public string ItmNb
    {
        get => this.itmNbField;
        set => this.itmNbField = value;
    }

    /// <remarks />
    public string Vrsn
    {
        get => this.vrsnField;
        set => this.vrsnField = value;
    }

    /// <remarks />
    public CryptographicKeyType3Code Tp
    {
        get => this.tpField;
        set => this.tpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool TpSpecified
    {
        get => this.tpFieldSpecified;
        set => this.tpFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("Fctn")]
    public KeyUsage1Code[] Fctn
    {
        get => this.fctnField;
        set => this.fctnField = value;
    }

    /// <remarks />
    public DateTime ActvtnDt
    {
        get => this.actvtnDtField;
        set => this.actvtnDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ActvtnDtSpecified
    {
        get => this.actvtnDtFieldSpecified;
        set => this.actvtnDtFieldSpecified = value;
    }

    /// <remarks />
    public DateTime DeactvtnDt
    {
        get => this.deactvtnDtField;
        set => this.deactvtnDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool DeactvtnDtSpecified
    {
        get => this.deactvtnDtFieldSpecified;
        set => this.deactvtnDtFieldSpecified = value;
    }

    /// <remarks />
    public ContentInformationType23 KeyVal
    {
        get => this.keyValField;
        set => this.keyValField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] KeyChckVal
    {
        get => this.keyChckValField;
        set => this.keyChckValField = value;
    }

    /// <remarks />
    [XmlElement("AddtlMgmtInf")]
    public GenericInformation1[] AddtlMgmtInf
    {
        get => this.addtlMgmtInfField;
        set => this.addtlMgmtInfField = value;
    }

    #endregion
}