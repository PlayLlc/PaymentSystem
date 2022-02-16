﻿using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.001.001.09")]
public partial class PartyIdentification178Choice
{
    #region Instance Values

    private object itemField;

    /// <remarks />
    [XmlElement("AnyBIC", typeof(string))]
    [XmlElement("NmAndAdr", typeof(NameAndAddress6))]
    [XmlElement("PrtryId", typeof(GenericIdentification36))]
    public object Item
    {
        get => this.itemField;
        set => this.itemField = value;
    }

    #endregion
}