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
public partial class CardAccount15
{
    #region Instance Values

    private AccountChoiceMethod1Code selctnMtdField;
    private bool selctnMtdFieldSpecified;
    private CardAccountType3Code selctdAcctTpField;
    private bool selctdAcctTpFieldSpecified;
    private string acctNmField;
    private NameAndAddress3 acctOwnrField;
    private string ccyField;
    private AccountIdentification39Choice acctIdrField;
    private PartyIdentification177Choice svcrField;

    /// <remarks />
    public AccountChoiceMethod1Code SelctnMtd
    {
        get => this.selctnMtdField;
        set => this.selctnMtdField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SelctnMtdSpecified
    {
        get => this.selctnMtdFieldSpecified;
        set => this.selctnMtdFieldSpecified = value;
    }

    /// <remarks />
    public CardAccountType3Code SelctdAcctTp
    {
        get => this.selctdAcctTpField;
        set => this.selctdAcctTpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SelctdAcctTpSpecified
    {
        get => this.selctdAcctTpFieldSpecified;
        set => this.selctdAcctTpFieldSpecified = value;
    }

    /// <remarks />
    public string AcctNm
    {
        get => this.acctNmField;
        set => this.acctNmField = value;
    }

    /// <remarks />
    public NameAndAddress3 AcctOwnr
    {
        get => this.acctOwnrField;
        set => this.acctOwnrField = value;
    }

    /// <remarks />
    public string Ccy
    {
        get => this.ccyField;
        set => this.ccyField = value;
    }

    /// <remarks />
    public AccountIdentification39Choice AcctIdr
    {
        get => this.acctIdrField;
        set => this.acctIdrField = value;
    }

    /// <remarks />
    public PartyIdentification177Choice Svcr
    {
        get => this.svcrField;
        set => this.svcrField = value;
    }

    #endregion
}