﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Interchange.Messages.DataFields._Temp;

// public class Track2DataMapper : VariableLengthDataFieldMapper { public static readonly DataFieldId DataFieldId = new(35); SHIIIIIIIIIIIIIIIIIIIIIIIIITprivate const ushort _MaxByteLength = 37; private const byte _LeadingOctetLength = 1; public override DataFieldId GetDataFieldId() => DataFieldId; public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId; protected override ushort GetMaxByteLength() => _MaxByteLength; protected override ushort GetLeadingOctetLength() => _LeadingOctetLength; }