using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;


public abstract record KnownObjects : EnumObject<Tag>
{public abstract bool Exists(Tag value);
    protected KnownObjects(Tag value) : base(value)
    { }
}
 