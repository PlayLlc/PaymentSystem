using Android.Views;

namespace PayWithPlay.Droid.Extensions
{
    public static class Extensions
    {
        public static bool IsDigitPressed(this Keycode keycode)
        {
            return keycode == Keycode.Num0 ||
                   keycode == Keycode.Numpad0 ||
                   keycode == Keycode.Num1 ||
                   keycode == Keycode.Numpad1 ||
                   keycode == Keycode.Num2 ||
                   keycode == Keycode.Numpad2 ||
                   keycode == Keycode.Num3 ||
                   keycode == Keycode.Numpad3 ||
                   keycode == Keycode.Num4 ||
                   keycode == Keycode.Numpad4 ||
                   keycode == Keycode.Num5 ||
                   keycode == Keycode.Numpad5 ||
                   keycode == Keycode.Num6 ||
                   keycode == Keycode.Numpad6 ||
                   keycode == Keycode.Num7 ||
                   keycode == Keycode.Numpad7 ||
                   keycode == Keycode.Num8 ||
                   keycode == Keycode.Numpad8 ||
                   keycode == Keycode.Num9 ||
                   keycode == Keycode.Numpad9;
        }
    }
}
