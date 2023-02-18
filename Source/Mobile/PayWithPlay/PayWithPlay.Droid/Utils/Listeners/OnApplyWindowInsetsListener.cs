using Android.Views;
using AndroidX.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Views.ViewGroup;

namespace PayWithPlay.Droid.Utils.Listeners
{
    public class OnApplyWindowInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        public WindowInsetsCompat OnApplyWindowInsets(View view, WindowInsetsCompat windowInsets)
        {
            var insets = windowInsets.GetInsets(WindowInsetsCompat.Type.SystemBars());

            // Apply the insets as a margin to the view. Here the system is setting
            // only the bottom, left, and right dimensions, but apply whichever insets are
            // appropriate to your layout. You can also update the view padding
            // if that's more appropriate.

            var lp = view.LayoutParameters as MarginLayoutParams;
            lp!.LeftMargin = insets.Left;
            lp!.RightMargin = insets.Right;
            lp!.BottomMargin = insets.Bottom;
            view.LayoutParameters = lp;

            // Return CONSUMED if you don't want want the window insets to keep being
            // passed down to descendant views.
            return WindowInsetsCompat.Consumed;
        }
    }
}