using AndroidX.RecyclerView.Widget;
using Android.Graphics;
using Google.Android.Material.Shape;
using AndroidX.Core.Content;
using Android.Content.Res;

namespace PayWithPlay.Droid.Utils
{
    public class RecyclerItemDecoration : RecyclerView.ItemDecoration
    {
        private readonly int _height;
        private readonly int _colorResId;
        private readonly int _marginStart;
        private readonly int _marginEnd;

        public RecyclerItemDecoration(int height, int colorResId, int marginStart, int marginEnd)
        {
            _height = height;
            _colorResId = colorResId;
            _marginStart = marginStart;
            _marginEnd = marginEnd;
        }

        public override void OnDrawOver(Canvas c, RecyclerView parent, RecyclerView.State state)
        {
            int dividerLeft = parent.PaddingLeft;
            int dividerRight = parent.Width - parent.PaddingRight;

            var divider = new MaterialShapeDrawable();
            divider.FillColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(App.Context, _colorResId)));

            for (int i = 0; i < parent.ChildCount - 1; i++)
            {
                var child = parent.GetChildAt(i)!;
                var lp = (RecyclerView.LayoutParams)child.LayoutParameters!;
                int dividerTop = child.Bottom + lp.BottomMargin;
                int dividerBottom = dividerTop + _height;
                divider.SetBounds(dividerLeft + _marginStart, dividerTop, dividerRight - _marginEnd, dividerBottom);
                divider.Draw(c);
            }
        }
    }
}