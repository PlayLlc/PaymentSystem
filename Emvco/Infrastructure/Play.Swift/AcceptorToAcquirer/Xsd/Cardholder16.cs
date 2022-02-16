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
public partial class Cardholder16
{
    #region Instance Values

    private PersonIdentification15 idField;
    private string nmField;
    private string langField;
    private PostalAddress22 bllgAdrField;
    private PostalAddress22 shppgAdrField;
    private string tripNbField;
    private Vehicle1 vhclField;
    private CardholderAuthentication13[] authntcnField;
    private TransactionVerificationResult4[] txVrfctnRsltField;
    private string prsnlDataField;
    private MobileData2[] mobDataField;

    /// <remarks />
    public PersonIdentification15 Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public string Nm
    {
        get => this.nmField;
        set => this.nmField = value;
    }

    /// <remarks />
    public string Lang
    {
        get => this.langField;
        set => this.langField = value;
    }

    /// <remarks />
    public PostalAddress22 BllgAdr
    {
        get => this.bllgAdrField;
        set => this.bllgAdrField = value;
    }

    /// <remarks />
    public PostalAddress22 ShppgAdr
    {
        get => this.shppgAdrField;
        set => this.shppgAdrField = value;
    }

    /// <remarks />
    public string TripNb
    {
        get => this.tripNbField;
        set => this.tripNbField = value;
    }

    /// <remarks />
    public Vehicle1 Vhcl
    {
        get => this.vhclField;
        set => this.vhclField = value;
    }

    /// <remarks />
    [XmlElement("Authntcn")]
    public CardholderAuthentication13[] Authntcn
    {
        get => this.authntcnField;
        set => this.authntcnField = value;
    }

    /// <remarks />
    [XmlElement("TxVrfctnRslt")]
    public TransactionVerificationResult4[] TxVrfctnRslt
    {
        get => this.txVrfctnRsltField;
        set => this.txVrfctnRsltField = value;
    }

    /// <remarks />
    public string PrsnlData
    {
        get => this.prsnlDataField;
        set => this.prsnlDataField = value;
    }

    /// <remarks />
    [XmlElement("MobData")]
    public MobileData2[] MobData
    {
        get => this.mobDataField;
        set => this.mobDataField = value;
    }

    #endregion
}