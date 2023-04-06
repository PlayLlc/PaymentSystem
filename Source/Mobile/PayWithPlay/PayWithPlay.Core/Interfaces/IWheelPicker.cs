namespace PayWithPlay.Core.Interfaces
{
    public interface IWheelPicker
    {
        void Show(string[] values, int startIndex, string? title = null, string? positiveButtonTitle = null, string? negativeButtonTitle = null, Action<int>? onPositiveAction = null);
    }
}
