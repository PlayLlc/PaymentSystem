using Android.Graphics;
using Android.Runtime;
using MikePhil.Charting.Components;
using MikePhil.Charting.Renderer;
using MikePhil.Charting.Util;

namespace PayWithPlay.Droid.Utils.Chart
{
    public class ObliqueLabelXAxisRenderer : XAxisRenderer
    {
        public ObliqueLabelXAxisRenderer(ViewPortHandler viewPortHandler, XAxis xAxis, Transformer trans) : base(viewPortHandler, xAxis, trans)
        {
        }

        protected ObliqueLabelXAxisRenderer(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void DrawLabel(Canvas? c, string? formattedLabel, float x, float y, MPPointF? anchor, float angleDegrees)
        {
            float drawOffsetX = .0f;
            float drawOffsetY = .0f;

            var fontMetricsBuffer = new Paint.FontMetrics();
            var drawTextRectBuffer = new Rect();

            var lineHeight = MAxisLabelPaint!.GetFontMetrics(fontMetricsBuffer);
            MAxisLabelPaint.GetTextBounds(formattedLabel, 0, formattedLabel!.Length, drawTextRectBuffer);

            // Android sometimes has pre-padding
            drawOffsetX -= drawTextRectBuffer.Left;

            // Android does not snap the bounds to line boundaries,
            //  and draws from bottom to top.
            // And we want to normalize it.
            drawOffsetY += -fontMetricsBuffer.Ascent;

            // To have a consistent point of reference, we always draw left-aligned
            var originalTextAlign = MAxisLabelPaint.TextAlign;
            MAxisLabelPaint.TextAlign = Paint.Align.Left;

            if (angleDegrees != .0f)
            {

                // Move the text drawing rect in a way that it always rotates around its center
                drawOffsetX -= drawTextRectBuffer.Width();// * 0.5f;
                drawOffsetY -= lineHeight * 0.5f;

                float translateX = x;
                float translateY = y;

                // Move the "outer" rect relative to the anchor, assuming its centered
                if (anchor!.X != 0.5f || anchor.Y != 0.5f)
                {
                    var rotatedSize = MikePhil.Charting.Util.Utils.GetSizeOfRotatedRectangleByDegrees(
                            drawTextRectBuffer.Width(),
                            lineHeight,
                            angleDegrees);

                    translateX -= rotatedSize!.Width * (anchor.X - 0.5f);
                    translateY -= rotatedSize.Height * anchor.Y;
                    FSize.RecycleInstance(rotatedSize);
                }

                c!.Save();
                c.Translate(translateX, translateY);
                c.Rotate(angleDegrees);

                c.DrawText(formattedLabel, drawOffsetX, drawOffsetY, MAxisLabelPaint);

                c.Restore();
            }
            else
            {
                if (anchor!.X != .0f || anchor.Y != .0f)
                {

                    drawOffsetX -= drawTextRectBuffer.Width() * anchor.X;
                    drawOffsetY -= lineHeight * anchor.Y;
                }
                drawOffsetX += x;
                drawOffsetY += y;

                c!.DrawText(formattedLabel, drawOffsetX, drawOffsetY, MAxisLabelPaint);
            }

            MAxisLabelPaint.TextAlign = originalTextAlign;
        }
    }
}
