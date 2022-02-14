﻿using System;
using System.Collections.Generic;

namespace Play.Emv.Terminal.Common.Tests;

internal class TerminalActionAnalysisServiceFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    public static IEnumerable<object[]> GetRandomTerminalActionCodeDenial(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeDenial()};
    }

    public static IEnumerable<object[]> GetRandomTerminalActionCodeDefault(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeDefault()};
    }

    public static IEnumerable<object[]> GetRandomTerminalActionCodeOnline(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeOnline()};
    }

    public static IEnumerable<object[]> GetRandomIssuerActionCodeOnline(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeOnline()};
    }

    public static IEnumerable<object[]> GetRandomIssuerActionCodeDefault(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeDefault()};
    }

    public static IEnumerable<object[]> GetRandomIssuerActionCodeDenial(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeDenial()};
    }

    #endregion
}