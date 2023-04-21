﻿using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using AndroidX.Camera.Core;
using PayWithPlay.Droid.Extensions;
using Path = global::Android.Graphics.Path;

namespace PayWithPlay.Droid.CustomViews
{
    public class ConfirmPhotoViewport : FrameLayout
    {
        private static readonly int _marginViewport = 24f.ToPx();
        private static readonly int _cornerLength = (int)(App.Context!.Resources!.DisplayMetrics!.WidthPixels * 0.288f);

        private readonly Paint _paint = new();
        private readonly Paint _cornersPaint = new();

        #region ctors  

        public ConfirmPhotoViewport(Context context) : base(context)
        {
            SetWillNotDraw(false);
            SetLayerType(Android.Views.LayerType.Hardware, null);
            InitTransparentPaint();
            InitCornersPaint();
        }

        public ConfirmPhotoViewport(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
            SetWillNotDraw(false);
            SetLayerType(Android.Views.LayerType.Hardware, null);
            InitTransparentPaint();
            InitCornersPaint();
        }

        public ConfirmPhotoViewport(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            SetWillNotDraw(false);
            SetLayerType(Android.Views.LayerType.Hardware, null);
            InitTransparentPaint();
            InitCornersPaint();
        }

        public ConfirmPhotoViewport(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            SetWillNotDraw(false);
            SetLayerType(Android.Views.LayerType.Hardware, null);
            InitTransparentPaint();
            InitCornersPaint();
        }

        protected ConfirmPhotoViewport(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            SetWillNotDraw(false);
            SetLayerType(Android.Views.LayerType.Hardware, null);
            InitTransparentPaint();
            InitCornersPaint();
        }

        #endregion

        public Rect? ViewportRect { get; set; }

        public override bool ShouldDelayChildPressedState()
        {
            return false;
        }

        public override void Draw(Canvas? canvas)
        {
            base.Draw(canvas);
        }

        protected override void DispatchDraw(Canvas? canvas)
        {
            canvas!.DrawColor(Color.Argb(128, 0, 0, 0));

            var left = _marginViewport;
            var top = (Height - Width + (2 * _marginViewport)) / 2;
            var right = Width - _marginViewport;
            var bottom = top + (Width - (2 * _marginViewport));

            var viewport = new Rect(left, top, right, bottom);
            ViewportRect = viewport;

            // draw transparent viewport
            canvas.DrawRect(viewport, _paint);

            canvas!.DrawPath(GetTopLeftPath(viewport), _cornersPaint);
            canvas.DrawPath(GetTopRightPath(viewport), _cornersPaint);
            canvas.DrawPath(GetBottomRightPath(viewport), _cornersPaint);
            canvas.DrawPath(GetBottomLeftPath(viewport), _cornersPaint);

            base.DispatchDraw(canvas);
        }

        private Path GetTopLeftPath(Rect viewport)
        {
            var path = new Path();
            path.MoveTo(viewport.Left, viewport.Top + _cornerLength);
            path.LineTo(viewport.Left, viewport.Top);
            path.LineTo(viewport.Left + _cornerLength, viewport.Top);

            return path;
        }

        private Path GetTopRightPath(Rect viewport)
        {
            var path = new Path();
            path.MoveTo(viewport.Left + viewport.Width() - _cornerLength, viewport.Top);
            path.LineTo(viewport.Left + viewport.Width(), viewport.Top);
            path.LineTo(viewport.Left + viewport.Width(), viewport.Top + _cornerLength);

            return path;
        }

        private Path GetBottomRightPath(Rect viewport)
        {
            var path = new Path();
            path.MoveTo(viewport.Left + viewport.Width(), viewport.Top + viewport.Height() - _cornerLength);
            path.LineTo(viewport.Left + viewport.Width(), viewport.Top + viewport.Height());
            path.LineTo(viewport.Left + viewport.Width() - _cornerLength, viewport.Top + viewport.Height());

            return path;
        }

        private Path GetBottomLeftPath(Rect viewport)
        {
            var path = new Path();
            path.MoveTo(viewport.Left, viewport.Top + viewport.Height() - _cornerLength);
            path.LineTo(viewport.Left, viewport.Top + viewport.Height());
            path.LineTo(viewport.Left + _cornerLength, viewport.Top + viewport.Height());

            return path;
        }

        private void InitTransparentPaint()
        {
            _paint.SetStyle(Paint.Style.Fill);
            _paint.Color = Color.Argb(0, 255, 255, 255);
            _paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
        }

        private void InitCornersPaint()
        {
            _cornersPaint.Color = Color.White;
            _cornersPaint.StrokeWidth = 2f.ToPx();
            _cornersPaint.SetStyle(Paint.Style.Stroke);
            _cornersPaint.Dither = true;
            _cornersPaint.StrokeCap = Paint.Cap.Round;
            _cornersPaint.SetPathEffect(new CornerPathEffect(10));
            _cornersPaint.AntiAlias = true;
        }
    }
}
