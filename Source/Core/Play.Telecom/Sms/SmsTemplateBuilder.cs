namespace Play.Telecom.Twilio.Sms;

public abstract class SmsTemplateBuilder
{
    #region Static Metadata

    protected const string _DynamicVariableStartSentinels = "<!--*";
    protected const string _DynamicVariableEndSentinels = "*-->";

    #endregion

    #region Constructor

    #endregion

    #region Instance Members

    protected abstract string GetTemplate();

    private static string GetTemplateVariable(string variableName) => $"{_DynamicVariableStartSentinels}{variableName}{_DynamicVariableEndSentinels}";

    protected string CreateMessage(Dictionary<string, string> dynamicValues)
    {
        string result = GetTemplate();
        foreach (KeyValuePair<string, string> dynamicValue in dynamicValues)
            result.Replace(GetTemplateVariable(dynamicValue.Key), dynamicValue.Value);

        return result;
    }

    #endregion
}