using Android.Content;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.Button;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using PayWithPlay.Android.Activities;
using PayWithPlay.Android.CustomViews;
using PayWithPlay.Core.ViewModels;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;

namespace PayWithPlay.Android.Fragments.UserRegistration
{
    public class VerifyIdentityFragment<TViewModel> : MvxFragment<TViewModel> where TViewModel : BaseVerifyIdentityViewModel
    {
        private InputBoxesView? _inputBoxes;
        private MaterialButton? _verifyBtn;
        private MaterialButton? _resendCodeBtn;

        public VerifyIdentityFragment()
        {
        }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var root = this.BindingInflate(Resource.Layout.fragment_verify_identity, container, false);

            InitViews(root);
            SetBindings(root);

            return root;
        }

        private void SetBindings(View root)
        {
            var title = root.FindViewById<TextView>(Resource.Id.title_textView)!;
            var subTitle = root.FindViewById<TextView>(Resource.Id.subTitle_textView)!;
            var message = root.FindViewById<TextView>(Resource.Id.message_textView)!;
            var expiresInTextView = root.FindViewById<AppCompatTextView>(Resource.Id.expires_in_tv)!;

            var identityImage = root.FindViewById<AppCompatImageView>(Resource.Id.identity_image)!;
            if (ViewModel.VerifyIdentityType == BaseVerifyIdentityViewModel.VerifyIdentity.Email)
            {
                identityImage.SetImageResource(Resource.Drawable.verify_email_image);
            }
            else
            {
                identityImage.SetImageResource(Resource.Drawable.verify_phone_number_image);
            }

            //title.Text = ViewModel.Title;
            subTitle.Text = ViewModel.Subtitle;
            message.Text = ViewModel.Message;
            _verifyBtn!.Text = BaseVerifyIdentityViewModel.VerifyButtonText;

            expiresInTextView.Text = BaseVerifyIdentityViewModel.ExipresAfter;
            _resendCodeBtn!.Text = BaseVerifyIdentityViewModel.ResendCodeButtonText;

            using (var set  = this.CreateBindingSet<VerifyIdentityFragment<TViewModel>, TViewModel>())
            {
                set.Bind(_inputBoxes).For(t => t!.TextValue).To(vm => vm.InputValue);
                set.Bind(title).For(t => t!.Text).To(vm => vm.Title);
            }
        }

        private void InitViews(View root)
        {
            _inputBoxes = root.FindViewById<InputBoxesView>(Resource.Id.inputsView)!;
            _verifyBtn = root.FindViewById<MaterialButton>(Resource.Id.verify_btn)!;
            _resendCodeBtn = root.FindViewById<MaterialButton>(Resource.Id.resend_code_btn)!;
        }
    }
}
