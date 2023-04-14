using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using AndroidX.Core.Content;
using Java.Interop;
using MvvmCross.Platforms.Android.Binding.Target;
using static Android.Widget.TextView;

namespace PayWithPlay.Droid.CustomBindings
{
    public class RedAsteriskBinding : MvxAndroidTargetBinding<TextView, bool>
    {
        public const string Property = "RedAsterisk";

        public RedAsteriskBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, bool value)
        {
            if(target == null)
            {
                return;
            }

            var text = $"{target.Text}*";

            target!.SetText(text, BufferType.Spannable);
            var spannable = target.TextFormatted.JavaCast<ISpannable>()!;

            //var textTypeface = target.Typeface;
            //spannable.SetSpan(new CustomTypefaceSpan(textTypeface), 0, text.Length - 1, SpanTypes.ExclusiveExclusive);
            //spannable.SetSpan(new ForegroundColorSpan(new Color(target.CurrentTextColor)), 0, text.Length, SpanTypes.ExclusiveExclusive);
            var negativeColor = new Color(ContextCompat.GetColor(target.Context, Resource.Color.negative_color));
            spannable.SetSpan(new ForegroundColorSpan(negativeColor), text.Length - 1, text.Length, SpanTypes.ExclusiveExclusive);
        }
    }
}
