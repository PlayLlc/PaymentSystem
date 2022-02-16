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
public partial class ActionMessage7
{
    #region Instance Values

    private UserInterface4Code msgDstnField;
    private InformationQualify1Code infQlfrField;
    private bool infQlfrFieldSpecified;
    private OutputFormat3Code frmtField;
    private bool frmtFieldSpecified;
    private string msgCnttField;
    private ContentInformationType21 msgCnttSgntrField;
    private OutputBarcode1 outptBrcdField;
    private bool rspnReqrdFlgField;
    private bool rspnReqrdFlgFieldSpecified;
    private decimal minDispTmField;
    private bool minDispTmFieldSpecified;

    /// <remarks />
    public UserInterface4Code MsgDstn
    {
        get => this.msgDstnField;
        set => this.msgDstnField = value;
    }

    /// <remarks />
    public InformationQualify1Code InfQlfr
    {
        get => this.infQlfrField;
        set => this.infQlfrField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool InfQlfrSpecified
    {
        get => this.infQlfrFieldSpecified;
        set => this.infQlfrFieldSpecified = value;
    }

    /// <remarks />
    public OutputFormat3Code Frmt
    {
        get => this.frmtField;
        set => this.frmtField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool FrmtSpecified
    {
        get => this.frmtFieldSpecified;
        set => this.frmtFieldSpecified = value;
    }

    /// <remarks />
    public string MsgCntt
    {
        get => this.msgCnttField;
        set => this.msgCnttField = value;
    }

    /// <remarks />
    public ContentInformationType21 MsgCnttSgntr
    {
        get => this.msgCnttSgntrField;
        set => this.msgCnttSgntrField = value;
    }

    /// <remarks />
    public OutputBarcode1 OutptBrcd
    {
        get => this.outptBrcdField;
        set => this.outptBrcdField = value;
    }

    /// <remarks />
    public bool RspnReqrdFlg
    {
        get => this.rspnReqrdFlgField;
        set => this.rspnReqrdFlgField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool RspnReqrdFlgSpecified
    {
        get => this.rspnReqrdFlgFieldSpecified;
        set => this.rspnReqrdFlgFieldSpecified = value;
    }

    /// <remarks />
    public decimal MinDispTm
    {
        get => this.minDispTmField;
        set => this.minDispTmField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool MinDispTmSpecified
    {
        get => this.minDispTmFieldSpecified;
        set => this.minDispTmFieldSpecified = value;
    }

    #endregion
}