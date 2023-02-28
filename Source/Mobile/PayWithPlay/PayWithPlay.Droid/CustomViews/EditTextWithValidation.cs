using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Google.Android.Material.TextField;
using Java.Lang;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Droid.CustomViews
{
    public class EditTextWithValidation : EditTextWithClearFocus, View.IOnFocusChangeListener, ITextInputValidator, ITextWatcher
    {
        #region ctors

        public EditTextWithValidation(Context context) : base(context)
        {
            Init();
        }

        public EditTextWithValidation(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init();
        }

        public EditTextWithValidation(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        protected EditTextWithValidation(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        #endregion

        public string? Value => Text;

        public IList<IValidation>? Validations { get; set; }

        public void OnFocusChange(View? v, bool hasFocus)
        {
            if (hasFocus)
            {
                ResetError();
                return;
            }

            Text = Text!.Trim();
            PerformValidation(true, false);
        }

        #region ITextWatcher

        void ITextWatcher.AfterTextChanged(IEditable? s)
        {
            PerformValidation(false, true);
        }

        void ITextWatcher.BeforeTextChanged(ICharSequence? s, int start, int count, int after)
        {
        }

        void ITextWatcher.OnTextChanged(ICharSequence? s, int start, int before, int count)
        {
        }

        #endregion

        public ValidationResult PerformValidation(bool displayError, bool hasFocus)
        {
            var result = GetValidationResult();
            if (result.IsValid)
            {
                ResetError();
                return result;
            }

            if (Text == string.Empty)
            {
                ResetError();
            }
            else if (displayError && !hasFocus)
            {
                SetInputLayoutError(result.Message);
                return result;
            }

            if (displayError && !hasFocus && string.IsNullOrWhiteSpace(Text))
            {
                SetInputLayoutError(result.Message);
            }

            return result;
        }

        public ValidationResult GetValidationResult()
        {
            if (Validations == null || Validations.Count == 0)
            {
                return ValidationResultFactory.Success;
            }

            foreach (var validation in Validations)
            {
                var result = validation.GetValidationResult(Text);
                if (!result.IsValid)
                {
                    return result;
                }
            }

            return ValidationResultFactory.Success;
        }

        private void SetInputLayoutError(string message)
        {
            if (Parent == null ||
                Parent.Parent is not TextInputLayout textInputLayout)
            {
                return;
            }

            if (!textInputLayout.ErrorEnabled)
            {
                Application.SynchronizationContext.Post(_ =>
                {
                    textInputLayout.Error = message;
                    textInputLayout.ErrorEnabled = true;
                }, null);
            }
        }

        private void ResetError()
        {
            if (Parent == null ||
                Parent.Parent is not TextInputLayout textInputLayout)
            {
                return;
            }

            if (textInputLayout.ErrorEnabled)
            {
                Application.SynchronizationContext.Post(_ =>
                {
                    textInputLayout.ErrorEnabled = false;
                }, null);
            }
        }

        private void Init()
        {
            OnFocusChangeListener = this;
            AddTextChangedListener(this);
        }
    }
}