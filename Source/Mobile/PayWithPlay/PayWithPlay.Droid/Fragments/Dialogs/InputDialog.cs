using Android.Text;
using Android.Views;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using PayWithPlay.Core.ViewModels;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.Dialogs
{
    public class InputDialog<TViewModel> : MvxDialogFragment<TViewModel> where TViewModel : DialogInputViewModel
    {
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetStyle(StyleNoTitle, Resource.Style.DialogFragmentTheme);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _ = base.OnCreateView(inflater, container, savedInstanceState);

            var root = this.BindingInflate(Resource.Layout.dialog_input, container, false);

            var inputLayout = root.FindViewById<TextInputLayout>(Resource.Id.value_til)!;
            var editText = root.FindViewById<EditTextWithValidation>(Resource.Id.value_et)!;

            inputLayout.PrefixText = ViewModel.Prefix;

            switch (ViewModel.InputType)
            {
                case DialogInputViewModel.InputTypeEnum.Text:
                    editText.InputType = InputTypes.ClassText | InputTypes.TextVariationNormal;
                    break;
                case DialogInputViewModel.InputTypeEnum.Decimal:
                    editText.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
                    break;
                case DialogInputViewModel.InputTypeEnum.Numeric:
                    editText.InputType = InputTypes.ClassNumber | InputTypes.NumberVariationNormal;
                    break;
            }

            root.SetBackground(Resource.Color.white, cornerRadius: 5f.ToFloatPx());

            return root;
        }
    }
}
