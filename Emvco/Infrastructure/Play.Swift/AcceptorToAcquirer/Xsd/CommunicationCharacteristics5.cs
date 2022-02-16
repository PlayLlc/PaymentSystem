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
public partial class CommunicationCharacteristics5
{
    #region Instance Values

    private POICommunicationType2Code comTpField;
    private PartyType7Code[] rmotPtyField;
    private bool actvField;
    private NetworkParameters7 paramsField;
    private PhysicalInterfaceParameter1 physIntrfcField;

    /// <remarks />
    public POICommunicationType2Code ComTp
    {
        get => this.comTpField;
        set => this.comTpField = value;
    }

    /// <remarks />
    [XmlElement("RmotPty")]
    public PartyType7Code[] RmotPty
    {
        get => this.rmotPtyField;
        set => this.rmotPtyField = value;
    }

    /// <remarks />
    public bool Actv
    {
        get => this.actvField;
        set => this.actvField = value;
    }

    /// <remarks />
    public NetworkParameters7 Params
    {
        get => this.paramsField;
        set => this.paramsField = value;
    }

    /// <remarks />
    public PhysicalInterfaceParameter1 PhysIntrfc
    {
        get => this.physIntrfcField;
        set => this.physIntrfcField = value;
    }

    #endregion
}