using Android.Content;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using PayWithPlay.Core.Constants;

namespace PayWithPlay.Droid
{
    public class CustomAndroidViewsContainer : MvxAndroidViewsContainer
    {
        public CustomAndroidViewsContainer(Context applicationContext) : base(applicationContext)
        {
        }

        protected override void AdjustIntentForPresentation(Intent intent, MvxViewModelRequest request)
        {
            base.AdjustIntentForPresentation(intent, request);

            if (request != null &&
                request.PresentationValues != null &&
                request.PresentationValues.ContainsKey(AppConstants.ClearBackStack))
            {
                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            }
        }
    }
}
