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
public partial class SaleContext4
{
    #region Instance Values

    private string saleIdField;
    private string saleRefNbField;
    private string saleRcncltnIdField;
    private string cshrIdField;
    private string[] cshrLangField;
    private string shftNbField;
    private bool cstmrOrdrReqFlgField;
    private bool cstmrOrdrReqFlgFieldSpecified;
    private string purchsOrdrNbField;
    private string invcNbField;
    private string dlvryNoteNbField;
    private Organisation26[] spnsrdMrchntField;
    private bool spltPmtField;
    private bool spltPmtFieldSpecified;
    private decimal rmngAmtField;
    private bool rmngAmtFieldSpecified;
    private bool forceOnlnFlgField;
    private bool forceOnlnFlgFieldSpecified;
    private bool reuseCardDataFlgField;
    private bool reuseCardDataFlgFieldSpecified;
    private CardDataReading8Code[] allwdNtryMdField;
    private SaleTokenScope1Code saleTknScpField;
    private bool saleTknScpFieldSpecified;
    private string addtlSaleDataField;

    /// <remarks />
    public string SaleId
    {
        get => this.saleIdField;
        set => this.saleIdField = value;
    }

    /// <remarks />
    public string SaleRefNb
    {
        get => this.saleRefNbField;
        set => this.saleRefNbField = value;
    }

    /// <remarks />
    public string SaleRcncltnId
    {
        get => this.saleRcncltnIdField;
        set => this.saleRcncltnIdField = value;
    }

    /// <remarks />
    public string CshrId
    {
        get => this.cshrIdField;
        set => this.cshrIdField = value;
    }

    /// <remarks />
    [XmlElement("CshrLang")]
    public string[] CshrLang
    {
        get => this.cshrLangField;
        set => this.cshrLangField = value;
    }

    /// <remarks />
    public string ShftNb
    {
        get => this.shftNbField;
        set => this.shftNbField = value;
    }

    /// <remarks />
    public bool CstmrOrdrReqFlg
    {
        get => this.cstmrOrdrReqFlgField;
        set => this.cstmrOrdrReqFlgField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool CstmrOrdrReqFlgSpecified
    {
        get => this.cstmrOrdrReqFlgFieldSpecified;
        set => this.cstmrOrdrReqFlgFieldSpecified = value;
    }

    /// <remarks />
    public string PurchsOrdrNb
    {
        get => this.purchsOrdrNbField;
        set => this.purchsOrdrNbField = value;
    }

    /// <remarks />
    public string InvcNb
    {
        get => this.invcNbField;
        set => this.invcNbField = value;
    }

    /// <remarks />
    public string DlvryNoteNb
    {
        get => this.dlvryNoteNbField;
        set => this.dlvryNoteNbField = value;
    }

    /// <remarks />
    [XmlElement("SpnsrdMrchnt")]
    public Organisation26[] SpnsrdMrchnt
    {
        get => this.spnsrdMrchntField;
        set => this.spnsrdMrchntField = value;
    }

    /// <remarks />
    public bool SpltPmt
    {
        get => this.spltPmtField;
        set => this.spltPmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SpltPmtSpecified
    {
        get => this.spltPmtFieldSpecified;
        set => this.spltPmtFieldSpecified = value;
    }

    /// <remarks />
    public decimal RmngAmt
    {
        get => this.rmngAmtField;
        set => this.rmngAmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool RmngAmtSpecified
    {
        get => this.rmngAmtFieldSpecified;
        set => this.rmngAmtFieldSpecified = value;
    }

    /// <remarks />
    public bool ForceOnlnFlg
    {
        get => this.forceOnlnFlgField;
        set => this.forceOnlnFlgField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ForceOnlnFlgSpecified
    {
        get => this.forceOnlnFlgFieldSpecified;
        set => this.forceOnlnFlgFieldSpecified = value;
    }

    /// <remarks />
    public bool ReuseCardDataFlg
    {
        get => this.reuseCardDataFlgField;
        set => this.reuseCardDataFlgField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool ReuseCardDataFlgSpecified
    {
        get => this.reuseCardDataFlgFieldSpecified;
        set => this.reuseCardDataFlgFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("AllwdNtryMd")]
    public CardDataReading8Code[] AllwdNtryMd
    {
        get => this.allwdNtryMdField;
        set => this.allwdNtryMdField = value;
    }

    /// <remarks />
    public SaleTokenScope1Code SaleTknScp
    {
        get => this.saleTknScpField;
        set => this.saleTknScpField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool SaleTknScpSpecified
    {
        get => this.saleTknScpFieldSpecified;
        set => this.saleTknScpFieldSpecified = value;
    }

    /// <remarks />
    public string AddtlSaleData
    {
        get => this.addtlSaleDataField;
        set => this.addtlSaleDataField = value;
    }

    #endregion
}