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
public partial class GenericIdentification177
{
    #region Instance Values

    private string idField;
    private PartyType33Code tpField;
    private bool tpFieldSpecified;
    private PartyType33Code issrField;
    private bool issrFieldSpecified;
    private string ctryField;
    private string shrtNmField;
    private NetworkParameters7 rmotAccsField;
    private Geolocation1 glctnField;

    /// <remarks />
    public string Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public PartyType33Code Tp
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
    public PartyType33Code Issr
    {
        get => this.issrField;
        set => this.issrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool IssrSpecified
    {
        get => this.issrFieldSpecified;
        set => this.issrFieldSpecified = value;
    }

    /// <remarks />
    public string Ctry
    {
        get => this.ctryField;
        set => this.ctryField = value;
    }

    /// <remarks />
    public string ShrtNm
    {
        get => this.shrtNmField;
        set => this.shrtNmField = value;
    }

    /// <remarks />
    public NetworkParameters7 RmotAccs
    {
        get => this.rmotAccsField;
        set => this.rmotAccsField = value;
    }

    /// <remarks />
    public Geolocation1 Glctn
    {
        get => this.glctnField;
        set => this.glctnField = value;
    }

    #endregion
}