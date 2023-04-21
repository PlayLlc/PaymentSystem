using Android.Graphics;
using Android.Views;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels;
using PayWithPlay.Core.ViewModels.Main;
using PayWithPlay.Droid.Extensions;
using Xamarin.Android.TouchImageView;
using PayWithPlay.Droid.CustomViews;
using Bumptech.Glide.Request.Target;
using PayWithPlay.Droid.Utils.Listeners;
using Android.Graphics.Drawables;
using PayWithPlay.Droid.Utils;
using Google.Android.Material.BottomSheet;

namespace PayWithPlay.Droid.Fragments.BottomSheets
{
    [MvxDialogFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel))]
    public class ConfirmPhotoBottomSheet : FullBottomSheet<ConfirmPhotoViewModel>
    {
        private TouchImageView? _touchImageView;
        private ConfirmPhotoViewport? _confirmPhotoVieport;

        public override int LayoutId => Resource.Layout.fragment_confirm_photo;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.CropImageTask = () => Task.Run(CropImageAsync);
        }


        public override void OnShow(object? sender, EventArgs e)
        {
            base.OnShow(sender, e);

            var bottomSheet = Dialog!.FindViewById(Resource.Id.design_bottom_sheet)!;
            var bottomSheetBehavior = (DraggableBottomSheetBehavior)BottomSheetBehavior.From(bottomSheet);
            bottomSheetBehavior.PreventDragView = View!.FindViewById<View>(Resource.Id.touchImage_containerView);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            _touchImageView = root.FindViewById<TouchImageView>(Resource.Id.touchImageView)!;
            _confirmPhotoVieport = root.FindViewById<ConfirmPhotoViewport>(Resource.Id.viewport)!;
            _touchImageView.ClipToOutline = false;

            Glide.With(this)
            .Load(ViewModel.Data!.ImageUrl)
            .SetDiskCacheStrategy(DiskCacheStrategy.None)
            .SkipMemoryCache(true)
            .Placeholder(Resource.Drawable.ic_image_placeholder)
            .Listener(new GlideRequestListener((resource, model, target, dataSource, isFirst) =>
            {
                var drawable = (BitmapDrawable)resource;
                _touchImageView.MinZoom = drawable.IntrinsicHeight / (float)drawable.IntrinsicWidth;
            }))
            .Into(new DrawableImageViewTarget(_touchImageView));
            _touchImageView.NestedScrollingEnabled = false;

            _confirmPhotoVieport.Post(() =>
            {
                var imgLp = _touchImageView.LayoutParameters!;
                imgLp.Width = _confirmPhotoVieport!.ViewportRect!.Width();
                imgLp.Height = _confirmPhotoVieport!.ViewportRect!.Height();
                _touchImageView.LayoutParameters = imgLp;
                _touchImageView.SetZoom((float)((View)_touchImageView.Parent!).Height / _confirmPhotoVieport!.ViewportRect!.Height());
            });

            return root;
        }

        private Task CropImageAsync()
        {
            var img = Glide.With(this)
            .AsBitmap()
            .SetDiskCacheStrategy(DiskCacheStrategy.None)
            .SkipMemoryCache(true)
            .Load(ViewModel.Data!.ImageUrl)
            .Submit().Get() as Bitmap;

            var finalImage = GetCurrentImageFromSource(img, _touchImageView!.ZoomedRect)!;

            try
            {
                var outStream = new FileStream(ViewModel.Data!.ImageUrl, FileMode.Open);
                finalImage.Compress(Bitmap.CompressFormat.Jpeg, 100, outStream);
                outStream.Flush();
                outStream.Close();
            }
            catch (Exception)
            {
            }

            return Task.CompletedTask;
        }

        private Bitmap? GetCurrentImageFromSource(Bitmap bitmap, RectF r)
        {
            if (r.Width() <= 0f || r.Height() <= 0f)
            {
                return null;
            }
            var originalImgWidth = bitmap.Width;
            var originalImgHeight = bitmap.Height;
            var x = originalImgWidth * r.Left;
            var y = originalImgHeight * r.Top;
            var newWidth = originalImgWidth * r.Width();
            var newHeight = originalImgHeight * r.Height();

            var resultBitmap = Bitmap.CreateBitmap(bitmap, (int)x, (int)y, (int)newWidth, (int)newHeight);

            var scaledBitmap = Bitmap.CreateScaledBitmap(resultBitmap, _touchImageView!.Width, _touchImageView.Height, true);
            resultBitmap.Recycle();
            resultBitmap = scaledBitmap;

            return resultBitmap;
        }
    }
}
