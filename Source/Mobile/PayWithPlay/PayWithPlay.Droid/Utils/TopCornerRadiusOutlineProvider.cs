using Android.Graphics;
using Android.Views;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Utils
{
    public class TopCornerRadiusOutlineProvider : ViewOutlineProvider
    {
        private readonly float _topCornerRadius;

        public TopCornerRadiusOutlineProvider(float topCornerRadius)
        {
            _topCornerRadius = topCornerRadius;
        }

        public override void GetOutline(View? view, Outline? outline)
        {
            outline?.SetRoundRect(0, 0, view!.Width, (int)(view.Height+ _topCornerRadius), _topCornerRadius);
        }
    }
}
