﻿namespace Play.Telecom.Twilio.Email;

public abstract class EmailTemplateBuilder
{
    #region Static Metadata

    protected const string _DynamicVariableStartSentinels = "<!--*";
    protected const string _DynamicVariableEndSentinels = "*-->";

    #endregion

    #region Instance Values

    public readonly string Subject;

    #endregion

    #region Constructor

    protected EmailTemplateBuilder(string subject)
    {
        Subject = subject;
    }

    #endregion

    #region Instance Members

    protected abstract string GetTemplate();

    private static string GetTemplateVariable(string variableName) => $"{_DynamicVariableStartSentinels}{variableName}{_DynamicVariableEndSentinels}";

    protected string CreateMessage(Dictionary<string, string> dynamicValues)
    {
        string result = GetTemplate();

        foreach (KeyValuePair<string, string> dynamicValue in dynamicValues)
        {
            var templateVariable = GetTemplateVariable(dynamicValue.Key);

            result = result.Replace(templateVariable, dynamicValue.Value);
        }

        return result;
    }

    #endregion
}