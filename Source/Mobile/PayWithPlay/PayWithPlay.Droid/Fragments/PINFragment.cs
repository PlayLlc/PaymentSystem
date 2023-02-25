using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels;
using PayWithPlay.Core.ViewModels.PIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using PayWithPlay.Droid.Activities;

namespace PayWithPlay.Droid.Fragments
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(GenericViewModel), FragmentContentId = Resource.Id.fragment_containerView, ViewModelType = typeof(PINViewModel))]
    public class PINFragment : BaseFragment<PINViewModel>
    {
        public PINFragment()
        {
        }

        public override int LayoutId => Resource.Layout.fragment_PIN;

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);

            if (Activity is GenericActivity genericActivity) 
            {
                genericActivity.SetTopImage(Resource.Drawable.enter_pin_top_image);
            }
        }
    }
}
