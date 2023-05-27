using Android.Text;
using PayWithPlay.Droid.Utils.InputFilters;

namespace PayWithPlay.Droid.Extensions
{
    public static class TextViewExtensions
    {
        public static void SetLimitedDecimalsInputFilter(this TextView textView, int maxDecimals = 2)
        {
            var currentFilters = textView.GetFilters();
            var limitFilter = new InputFilterWithLimitedDecimals(maxDecimals);
            if (currentFilters == null)
            {
                currentFilters = new IInputFilter[] { limitFilter };
            }
            else
            {
                currentFilters = currentFilters.Append(limitFilter).ToArray();
            }

            textView.SetFilters(currentFilters);
        }
    }
}
