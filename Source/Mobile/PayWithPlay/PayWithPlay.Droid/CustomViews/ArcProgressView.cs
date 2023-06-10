using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Org.W3c.Dom;
using PayWithPlay.Core.Extensions;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.CustomViews
{
    public class ArcProgressView : View
    {
        private const int START_ANGLE = 135;
        private const int SWEEP_ANGLE = 270;

        private int _pathStrokeWidth;
        private int _progressStrokeWidth;
        private int _indicatorStrokeWidth;
        private Color _pathColor;
        private Color _progressColor;
        private Color _indicatorColor;

        private Paint? _pathPaint;
        private Paint? _progressPaint;
        private Paint? _centerCirclePaint;
        private Paint? _indicatorLinePaint;

        private float currentProgress;
        private float targetProgress;
        private ValueAnimator? progressAnimator;

        #region ctros

        public ArcProgressView(Context? context) : base(context)
        {
            Init(null);
        }

        public ArcProgressView(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public ArcProgressView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(attrs);
        }

        public ArcProgressView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(attrs);
        }

        protected ArcProgressView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init(null);
        }

        #endregion

        public void SetProgress(float progress)
        {
            this.currentProgress = progress;
            Invalidate();
        }

        public void SetProgress(float progress, bool animated)
        {
            if (animated)
            {
                if (progressAnimator != null && progressAnimator.IsRunning)
                {
                    progressAnimator!.Cancel();
                }

                targetProgress = progress;
                progressAnimator = ValueAnimator.OfFloat(currentProgress, targetProgress)!;
                progressAnimator.SetDuration(500);
                progressAnimator.Update += (sender, args) =>
                {
                    SetProgress((int)args.Animation.AnimatedValue);
                };
                progressAnimator.Start();
            }
            else
            {
                SetProgress(progress);
            }
        }

        protected override void OnDraw(Canvas? canvas)
        {
            base.OnDraw(canvas);

            var width = Width;
            var height = Height;
            var minDimen = Math.Min(width, height);
            var strokeWidth = _progressStrokeWidth;
            var padding = strokeWidth / 2;

            var centerX = width / 2;
            var centerY = height / 2;

            var left = centerX - minDimen / 2 + padding;
            var top = centerY - minDimen / 2 + padding;
            var right = centerX + minDimen / 2 - padding;
            var bottom = centerY + minDimen / 2 - padding;

            canvas!.DrawArc(left, top, right, bottom, START_ANGLE, SWEEP_ANGLE, false, _pathPaint);
            canvas.DrawArc(left, top, right, bottom, START_ANGLE, currentProgress * SWEEP_ANGLE, false, _progressPaint);


            var indicatorRadius = _indicatorStrokeWidth;
            canvas.DrawCircle(centerX, centerY, indicatorRadius, _centerCirclePaint);

            var centerLineLength = minDimen / 2.6f - padding - strokeWidth / 2f;
            var indicatorAngle = (START_ANGLE + currentProgress * SWEEP_ANGLE).ToRadians();
            var centerLineEndX = centerX + centerLineLength * (float)Math.Cos(indicatorAngle);
            var centerLineEndY = centerY + centerLineLength * (float)Math.Sin(indicatorAngle);
            canvas.DrawLine(centerX, centerY, centerLineEndX, centerLineEndY, _indicatorLinePaint);
        }

        private void Init(IAttributeSet? attributeSet)
        {
            var attrs = Context!.Theme!.ObtainStyledAttributes(attributeSet, Resource.Styleable.ArcProgressView, 0, 0);
            try
            {
                _pathStrokeWidth = attrs.GetDimensionPixelSize(Resource.Styleable.ArcProgressView_pathStrokeWidth, 1.66f.ToPx());
                _progressStrokeWidth = attrs.GetDimensionPixelSize(Resource.Styleable.ArcProgressView_progressStrokeWidth, 4f.ToPx());
                _indicatorStrokeWidth = attrs.GetDimensionPixelSize(Resource.Styleable.ArcProgressView_progressStrokeWidth, 2f.ToPx());

                _pathColor = attrs.GetColor(Resource.Styleable.ArcProgressView_pathColor, Color.LightGray);
                _progressColor = attrs.GetColor(Resource.Styleable.ArcProgressView_progressColor, Color.Blue);
                _indicatorColor = attrs.GetColor(Resource.Styleable.ArcProgressView_indicatorColor, Color.Black);
            }
            finally
            {
                attrs.Recycle();
            }

            _pathPaint = new Paint
            {
                Color = _pathColor,
                StrokeWidth = _pathStrokeWidth,
                StrokeCap = Paint.Cap.Round,
                AntiAlias = true
            };
            _pathPaint.SetStyle(Paint.Style.Stroke);

            _progressPaint = new Paint
            {
                Color = _progressColor,
                StrokeWidth = _progressStrokeWidth,
                StrokeCap = Paint.Cap.Round,
                AntiAlias = true
            };
            _progressPaint.SetStyle(Paint.Style.Stroke);

            _centerCirclePaint = new Paint
            {
                Color = _indicatorColor,
                AntiAlias = true
            };
            _centerCirclePaint.SetStyle(Paint.Style.Fill);

            _indicatorLinePaint = new Paint
            {
                Color = _indicatorColor,
                StrokeWidth = _indicatorStrokeWidth,
                StrokeCap = Paint.Cap.Round,
                AntiAlias = true
            };
            _indicatorLinePaint.SetStyle(Paint.Style.Fill);

            currentProgress = 0f;
            targetProgress = 0f;
        }
    }
}
