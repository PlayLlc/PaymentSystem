﻿using Play.Core.Exceptions;

namespace Play.Underwriting.Application.Jobs;

internal class FileHelper
{
    #region Instance Members

    public static string SanitizeConsolidatedCsvListFile(string content, string fileName)
    {
        if (content.Length == 0)
            throw new PlayInternalException($"Empty {fileName} file");

        content = content.Substring(0, content.Length - 4);

        return content;
    }

    public static string SanitizeSanctionsListFile(string content)
    {
        if (content.Length == 0)
            throw new PlayInternalException("Empty sanctions list file");

        return content;
    }

    #endregion
}