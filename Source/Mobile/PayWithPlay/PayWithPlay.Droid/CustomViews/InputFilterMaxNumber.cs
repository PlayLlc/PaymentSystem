using Android.Text;
using Java.Lang;

namespace PayWithPlay.Droid.CustomViews
{
    public class InputFilterMaxNumber : Java.Lang.Object, IInputFilter
    {
        private int _maximumValue;

        public InputFilterMaxNumber(int maximumValue)
        {
            _maximumValue = maximumValue;
        }

        public ICharSequence? FilterFormatted(ICharSequence? source, int start, int end, ISpanned? dest, int dstart, int dend)
        {
            if (int.TryParse(dest.SubSequence(0, dstart) + source + dest.SubSequence(dend, dest.Length()), out int input) && 
                input <= _maximumValue)
            {
                return null;
            }

            return new Java.Lang.String("");
        }
    }
}
