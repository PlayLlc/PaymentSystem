using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Google.Android.Material.Button;

namespace PayWithPlay.Droid.CustomViews
{
    public class SingleLineButton : MaterialButton
    {
        #region ctors

        public SingleLineButton(Context context) : base(context)
        {
        }

        public SingleLineButton(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        public SingleLineButton(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected SingleLineButton(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        public override bool OnPreDraw()
        {
            var originalText = Text;

            if (!string.IsNullOrWhiteSpace(originalText))
            {
                var textPaint = Paint;

                // Get the available width for the text
                int availableWidth = MeasuredWidth - PaddingLeft - PaddingRight;

                // Create a StaticLayout to measure the text and find the number of lines required
                var sb = StaticLayout.Builder.Obtain(originalText, 0, originalText!.Length, textPaint, availableWidth)
                    .SetAlignment(Layout.Alignment.AlignNormal)
                    .SetLineSpacing(LineSpacingExtra, LineSpacingMultiplier)
                    .SetIncludePad(false);
                var staticLayout = sb.Build();

                // Check if the text exceeds the available width
                if (staticLayout.LineCount > 1)
                {
                    // Find the starting index of the last word in the original text
                    var lastWordStart = originalText.LastIndexOf(' ', staticLayout.GetLineEnd(0) - 1);

                    // If lastWordStart is valid (not -1) and it's less than the original text length
                    if (lastWordStart != -1 && lastWordStart < originalText.Length)
                    {
                        // Truncate the original text at the last word's starting index
                        var truncatedText = originalText.Substring(0, lastWordStart).Trim();

                        // Set the truncated text on the TextView
                        Text = truncatedText;

                        // Re-measure the text to adjust the layout and keep it centered
                        int widthSpec = MeasureSpec.MakeMeasureSpec(availableWidth, MeasureSpecMode.Exactly);
                        int heightSpec = MeasureSpec.MakeMeasureSpec(Height, MeasureSpecMode.Exactly);
                        Measure(widthSpec, heightSpec);
                        Layout(Left, Top, Right, Bottom);
                    }
                }
            }

            return base.OnPreDraw();
        }
    }
}