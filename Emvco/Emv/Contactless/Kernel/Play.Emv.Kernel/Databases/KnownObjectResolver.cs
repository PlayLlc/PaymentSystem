using System;
 
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Core;

namespace Play.Emv.Kernel.Databases;

public abstract record KnownObjectResolver : IEquatable<Tag>, IEqualityComparer<Tag>, IComparable<Tag>, IEqualityComparer<EnumObject<Tag>>,
    IComparable<EnumObject<Tag>>
{ 

    protected KnownObjectResolver( )
    { 
    }

    public abstract bool Exists(Tag value);
    public abstract bool Equals(Tag other);
    public abstract bool Equals(Tag x, Tag y);
    public abstract int GetHashCode(Tag obj);
    public abstract int CompareTo(Tag other);
    public abstract bool Equals(EnumObject<Tag>? x, EnumObject<Tag>? y);
    public abstract int GetHashCode(EnumObject<Tag> obj);
    public abstract int CompareTo(EnumObject<Tag>? other);
}