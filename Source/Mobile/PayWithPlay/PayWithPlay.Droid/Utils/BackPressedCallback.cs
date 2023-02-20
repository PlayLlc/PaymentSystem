using AndroidX.Activity;

namespace PayWithPlay.Droid.Utils
{
    public class BackPressedCallback : OnBackPressedCallback
    {
        public BackPressedCallback(Action? onBackPressedAction = null) : base(true)
        {
            OnBackPressedAction = onBackPressedAction;
        }

        public Action? OnBackPressedAction { get; set; }

        public override void HandleOnBackPressed()
        {
            OnBackPressedAction?.Invoke();
        }
    }
}