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
public partial class PointOfInteractionComponent10
{
    #region Instance Values

    private POIComponentType6Code tpField;
    private string subTpInfField;
    private PointOfInteractionComponentIdentification1 idField;
    private PointOfInteractionComponentStatus3 stsField;
    private GenericIdentification48[] stdCmplcField;
    private PointOfInteractionComponentCharacteristics6 chrtcsField;
    private PointOfInteractionComponentAssessment1[] assmntField;
    private PackageType1[] packgField;

    /// <remarks />
    public POIComponentType6Code Tp
    {
        get => this.tpField;
        set => this.tpField = value;
    }

    /// <remarks />
    public string SubTpInf
    {
        get => this.subTpInfField;
        set => this.subTpInfField = value;
    }

    /// <remarks />
    public PointOfInteractionComponentIdentification1 Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public PointOfInteractionComponentStatus3 Sts
    {
        get => this.stsField;
        set => this.stsField = value;
    }

    /// <remarks />
    [XmlElement("StdCmplc")]
    public GenericIdentification48[] StdCmplc
    {
        get => this.stdCmplcField;
        set => this.stdCmplcField = value;
    }

    /// <remarks />
    public PointOfInteractionComponentCharacteristics6 Chrtcs
    {
        get => this.chrtcsField;
        set => this.chrtcsField = value;
    }

    /// <remarks />
    [XmlElement("Assmnt")]
    public PointOfInteractionComponentAssessment1[] Assmnt
    {
        get => this.assmntField;
        set => this.assmntField = value;
    }

    /// <remarks />
    [XmlElement("Packg")]
    public PackageType1[] Packg
    {
        get => this.packgField;
        set => this.packgField = value;
    }

    #endregion
}