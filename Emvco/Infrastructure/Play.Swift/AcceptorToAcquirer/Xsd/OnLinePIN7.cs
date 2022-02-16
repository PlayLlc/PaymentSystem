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
public partial class OnLinePIN7
{
    #region Instance Values

    private ContentInformationType22 ncrptdPINBlckField;
    private PINFormat3Code pINFrmtField;
    private string addtlInptField;

    /// <remarks />
    public ContentInformationType22 NcrptdPINBlck
    {
        get => this.ncrptdPINBlckField;
        set => this.ncrptdPINBlckField = value;
    }

    /// <remarks />
    public PINFormat3Code PINFrmt
    {
        get => this.pINFrmtField;
        set => this.pINFrmtField = value;
    }

    /// <remarks />
    public string AddtlInpt
    {
        get => this.addtlInptField;
        set => this.addtlInptField = value;
    }

    #endregion
}