using Android.Content.Res;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Enums;

namespace PayWithPlay.Droid.CustomBindings
{
    public class BottomSheetIconTypeBinding : MvxAndroidTargetBinding<AppCompatImageView, BottomSheetIconType>
    {
        public const string Property = "BSIconType";

        public BottomSheetIconTypeBinding(AppCompatImageView target) : base(target)
        {
        }

        protected override void SetValueImpl(AppCompatImageView target, BottomSheetIconType value)
        {
            if (target == null)
            {
                return;
            }

            switch (value)
            {
                case BottomSheetIconType.Camera:
                    target.SetImageResource(Resource.Drawable.ic_camera);
                    target.ImageTintList = ColorStateList.ValueOf(new Android.Graphics.Color(ContextCompat.GetColor(target.Context, Resource.Color.accent_color)));
                    break;
                case BottomSheetIconType.PhotoLibrary:
                    target.SetImageResource(Resource.Drawable.ic_photo_library);
                    target.ImageTintList = ColorStateList.ValueOf(new Android.Graphics.Color(ContextCompat.GetColor(target.Context, Resource.Color.accent_color)));
                    break;
            }
        }
    }
}
