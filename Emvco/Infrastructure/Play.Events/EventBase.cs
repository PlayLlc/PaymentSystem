﻿using System;
using System.Text;

using Play.Core.Specifications;
using Play.Randoms;

namespace Play.Events;

public abstract class EventBase : IEquatable<EventBase>
{
    #region Instance Values

    public readonly ushort EventId = Randomize.Integers.UShort();

    #endregion

    #region Constructor

    protected EventBase()
    { }

    #endregion

    #region Instance Members

    protected static EventTypeId GetEventTypeId(Type eventType)
    {
        if (!eventType.IsAssignableFrom(typeof(EventBase)))
        {
            throw new ArgumentOutOfRangeException(nameof(eventType),
                                                  $"The argument {nameof(eventType)} was expected to inherit {nameof(EventBase)}");
        }

        string fullName = eventType.AssemblyQualifiedName!;
        ReadOnlySpan<byte> buffer = Encoding.Unicode.GetBytes(fullName);

        if (buffer.Length > Specs.Integer.Int64.ByteSize)
            return new EventTypeId(BitConverter.ToUInt16(buffer[..Specs.Integer.Int64.ByteSize]));

        Span<byte> resultBuffer = stackalloc byte[Specs.Integer.Int16.ByteSize];
        buffer.CopyTo(resultBuffer);

        return new EventTypeId(BitConverter.ToUInt16(resultBuffer));
    }

    public abstract EventTypeId GetEventTypeId();

    #endregion

    #region Equality

    public bool Equals(EventBase? other)
    {
        if (other == null)
            return false;

        return other.GetEventTypeId() == GetEventTypeId();
    }

    #endregion
}