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
public partial class CardPaymentTransactionDetails48
{
    #region Instance Values

    private string ccyField;
    private decimal ttlAmtField;
    private decimal cmltvAmtField;
    private bool cmltvAmtFieldSpecified;
    private TypeOfAmount8Code amtQlfrField;
    private bool amtQlfrFieldSpecified;
    private DetailedAmount15 dtldAmtField;
    private decimal reqdAmtField;
    private bool reqdAmtFieldSpecified;
    private decimal authrsdAmtField;
    private bool authrsdAmtFieldSpecified;
    private decimal invcAmtField;
    private bool invcAmtFieldSpecified;
    private DateTime vldtyDtField;
    private bool vldtyDtFieldSpecified;
    private OnLineReason1Code[] onLineRsnField;
    private string uattnddLvlCtgyField;
    private CardAccountType3Code acctTpField;
    private bool acctTpFieldSpecified;
    private CurrencyConversion17 ccyConvsRsltField;
    private RecurringTransaction2 instlmtField;
    private AggregationTransaction3 aggtnTxField;
    private string pdctCdSetIdField;
    private Product6[] saleItmField;
    private string dlvryLctnField;
    private ExternallyDefinedData1[] addtlInfField;
    private byte[] iCCRltdDataField;

    /// <remarks />
    public string Ccy
    {
        get => this.ccyField;
        set => this.ccyField = value;
    }

    /// <remarks />
    public decimal TtlAmt
    {
        get => this.ttlAmtField;
        set => this.ttlAmtField = value;
    }

    /// <remarks />
    public decimal CmltvAmt
    {
        get => this.cmltvAmtField;
        set => this.cmltvAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CmltvAmtSpecified
    {
        get => this.cmltvAmtFieldSpecified;
        set => this.cmltvAmtFieldSpecified = value;
    }

    /// <remarks />
    public TypeOfAmount8Code AmtQlfr
    {
        get => this.amtQlfrField;
        set => this.amtQlfrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AmtQlfrSpecified
    {
        get => this.amtQlfrFieldSpecified;
        set => this.amtQlfrFieldSpecified = value;
    }

    /// <remarks />
    public DetailedAmount15 DtldAmt
    {
        get => this.dtldAmtField;
        set => this.dtldAmtField = value;
    }

    /// <remarks />
    public decimal ReqdAmt
    {
        get => this.reqdAmtField;
        set => this.reqdAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ReqdAmtSpecified
    {
        get => this.reqdAmtFieldSpecified;
        set => this.reqdAmtFieldSpecified = value;
    }

    /// <remarks />
    public decimal AuthrsdAmt
    {
        get => this.authrsdAmtField;
        set => this.authrsdAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AuthrsdAmtSpecified
    {
        get => this.authrsdAmtFieldSpecified;
        set => this.authrsdAmtFieldSpecified = value;
    }

    /// <remarks />
    public decimal InvcAmt
    {
        get => this.invcAmtField;
        set => this.invcAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool InvcAmtSpecified
    {
        get => this.invcAmtFieldSpecified;
        set => this.invcAmtFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement(DataType = "date")]
    public DateTime VldtyDt
    {
        get => this.vldtyDtField;
        set => this.vldtyDtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool VldtyDtSpecified
    {
        get => this.vldtyDtFieldSpecified;
        set => this.vldtyDtFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("OnLineRsn")]
    public OnLineReason1Code[] OnLineRsn
    {
        get => this.onLineRsnField;
        set => this.onLineRsnField = value;
    }

    /// <remarks />
    public string UattnddLvlCtgy
    {
        get => this.uattnddLvlCtgyField;
        set => this.uattnddLvlCtgyField = value;
    }

    /// <remarks />
    public CardAccountType3Code AcctTp
    {
        get => this.acctTpField;
        set => this.acctTpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool AcctTpSpecified
    {
        get => this.acctTpFieldSpecified;
        set => this.acctTpFieldSpecified = value;
    }

    /// <remarks />
    public CurrencyConversion17 CcyConvsRslt
    {
        get => this.ccyConvsRsltField;
        set => this.ccyConvsRsltField = value;
    }

    /// <remarks />
    public RecurringTransaction2 Instlmt
    {
        get => this.instlmtField;
        set => this.instlmtField = value;
    }

    /// <remarks />
    public AggregationTransaction3 AggtnTx
    {
        get => this.aggtnTxField;
        set => this.aggtnTxField = value;
    }

    /// <remarks />
    public string PdctCdSetId
    {
        get => this.pdctCdSetIdField;
        set => this.pdctCdSetIdField = value;
    }

    /// <remarks />
    [XmlElement("SaleItm")]
    public Product6[] SaleItm
    {
        get => this.saleItmField;
        set => this.saleItmField = value;
    }

    /// <remarks />
    public string DlvryLctn
    {
        get => this.dlvryLctnField;
        set => this.dlvryLctnField = value;
    }

    /// <remarks />
    [XmlElement("AddtlInf")]
    public ExternallyDefinedData1[] AddtlInf
    {
        get => this.addtlInfField;
        set => this.addtlInfField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] ICCRltdData
    {
        get => this.iCCRltdDataField;
        set => this.iCCRltdDataField = value;
    }

    #endregion
}