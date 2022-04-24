using System;
using System.Collections.Generic;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

internal class TerminalActionAnalysisServiceFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Terminal And Issuer

    public static IEnumerable<object[]> GetRandomTerminalAndIssuerActionCodeOnline(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new object[]
            {
                TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeOnline(),
                TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeOnline()
            };
        }
    }

    public static IEnumerable<object[]> GetRandomTerminalAndIssuerActionCodeDefault(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new object[]
            {
                TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeDefault(),
                TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeDefault()
            };
        }
    }

    public static IEnumerable<object[]> GetRandomTerminalAndIssuerActionCodeDenial(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new object[]
            {
                TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeDefault(),
                TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeDefault()
            };
        }
    }

    #endregion

    #region Terminal

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

    #endregion

    #region Issuer

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