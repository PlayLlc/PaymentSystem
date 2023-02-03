using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Google.Android.Material.TextField;

namespace PayWithPlay.Android.CustomViews
{
    public class EditTextWithClearFocus : TextInputEditText
    {
        #region ctors

        public EditTextWithClearFocus(Context context) : base(context)
        {
        }

        public EditTextWithClearFocus(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        public EditTextWithClearFocus(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected EditTextWithClearFocus(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        public override bool OnKeyPreIme([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            if (keyCode == Keycode.Back)
            {
                ClearFocus();
            }

            return true;
        }

        public override void OnEditorAction([GeneratedEnum] ImeAction actionCode)
        {
            base.OnEditorAction(actionCode);
            if (actionCode == ImeAction.Go ||
                actionCode == ImeAction.Done ||
                actionCode == ImeAction.Search
)
            {
                ClearFocus();
            }
        }

        public override void ClearFocus()
        {
            DismissKeyboard();
            base.ClearFocus();
        }

        private void DismissKeyboard()
        {
            var imm = Context?.GetSystemService(Activity.InputMethodService) as InputMethodManager;
            if (imm != null)
            {
                imm.HideSoftInputFromWindow(this.WindowToken, 0);
            }
        }
    }
}