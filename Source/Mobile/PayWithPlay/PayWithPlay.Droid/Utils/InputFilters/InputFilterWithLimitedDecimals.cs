using Android.Text;
using Java.Lang;

namespace PayWithPlay.Droid.Utils.InputFilters
{
    public class InputFilterWithLimitedDecimals : Java.Lang.Object, IInputFilter
    {
        private int _maximumValue;

        public InputFilterWithLimitedDecimals(int maximumValue)
        {
            _maximumValue = maximumValue;
        }

        public ICharSequence? FilterFormatted(ICharSequence? source, int start, int end, ISpanned? dest, int dstart, int dend)
        {
            var finalValue = dest.SubSequence(0, dstart) + source + dest.SubSequence(dend, dest.Length());
            if ((finalValue.Contains('.') && finalValue.Length - finalValue.LastIndexOf(".") - 1 > _maximumValue) ||
                (finalValue.Contains(',') && finalValue.Length - finalValue.LastIndexOf(",") - 1 > _maximumValue))
            {
                return new Java.Lang.String("");
            }

            return null;
        }
    }
}
