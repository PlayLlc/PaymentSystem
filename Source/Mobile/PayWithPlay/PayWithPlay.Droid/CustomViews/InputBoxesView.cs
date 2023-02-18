using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Google.Android.Material.TextField;
using PayWithPlay.Droid.Extensions;
using System.ComponentModel;

namespace PayWithPlay.Droid.CustomViews
{
    public class InputBoxesView : LinearLayout, ViewTreeObserver.IOnPreDrawListener, INotifyPropertyChanged
    {
        private static readonly int _minInnerSpace = 4f.ToPx();
        private static readonly int _minInputWidth = 16f.ToPx();

        private static readonly int _defaultInputTextSize = 32;
        private static readonly int _defaultInputWidth = 38f.ToPx();
        private static readonly int _defaultInputHeight = 48f.ToPx();
        private static readonly int _defaultInnerSpace = 20f.ToPx();

        private readonly List<EditTextWithClearFocus> _editTexts = new();

        private int _inputTextSize = _defaultInputTextSize;
        private int _inputWidth = _defaultInputWidth;
        private int _inputHeight = _defaultInputHeight;
        private int _inputInnerSapce = _defaultInnerSpace;
        private int _inputsCount;

        private string? _textValue;

        #region ctors

        public InputBoxesView(Context? context) : base(context)
        {
        }

        public InputBoxesView(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public InputBoxesView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(attrs);
        }

        public InputBoxesView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(attrs);
        }

        protected InputBoxesView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? TextValue
        {
            get => _textValue;
            set
            {
                if (Equals(_textValue, value))
                {
                    return;
                }

                _textValue = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextValue)));
            }
        }

        private void Init(IAttributeSet attributeSet)
        {
            var attrs = Context!.Theme!.ObtainStyledAttributes(attributeSet, Resource.Styleable.InputBoxesView, 0, 0);
            try
            {
                _inputsCount = attrs.GetInt(Resource.Styleable.InputBoxesView_inputsCount, 0);
            }
            finally
            {
                attrs.Recycle();
            }

            SetGravity(GravityFlags.CenterHorizontal);
            Orientation = Orientation.Horizontal;

            ViewTreeObserver!.AddOnPreDrawListener(this);
        }

        private TextInputLayout GetEditTextInput(int index)
        {
            var contextTheme = new ContextThemeWrapper(Context, Resource.Style.TextInputTheme);

            var inputLayout = new TextInputLayout(contextTheme, null, Resource.Attribute.textInputStyle)
            {
                LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            };

            inputLayout.Id = Resource.Id.input_box;
            inputLayout.SetPadding(0, 0, 0, 0);
            inputLayout.SetMinimumHeight(0);
            inputLayout.SetMinimumWidth(0);

            var inputView = new EditTextWithClearFocus(contextTheme, null, Resource.Attribute.InputBoxesViewStyleAttr);
            inputLayout.AddView(inputView);

            inputView.Tag = index;
            inputView.ImeOptions = index != _inputsCount - 1 ? ImeAction.Next : ImeAction.Done;
            inputView.SetPadding(0, 0, 0, 0);
            inputView.SetMinimumHeight(0);
            inputView.SetMinimumWidth(0);
            inputView.SetTextSize(ComplexUnitType.Sp, _inputTextSize);

            var lp = inputView.LayoutParameters;
            lp!.Height = _inputHeight;
            lp.Width = _inputWidth;
            inputView.LayoutParameters = lp;

            inputView.AfterTextChanged += InputView_AfterTextChanged;
            inputView.KeyPress += InputView_KeyPress;
            inputView.TextChanged += InputView_TextChanged;

            _editTexts.Add(inputView);

            return inputLayout;
        }

        private void InputView_AfterTextChanged(object? sender, global::Android.Text.AfterTextChangedEventArgs e)
        {
            var currentFocusedView = (EditTextWithClearFocus)sender!;
            if (currentFocusedView.Text != null && currentFocusedView.Text.Length > 0)
            {
                if ((int)currentFocusedView.Tag == _inputsCount - 1)
                {
                    currentFocusedView.ClearFocus();
                }
                else
                {
                    var next = (EditText)currentFocusedView.FocusSearch(FocusSearchDirection.Right)!; // or FOCUS_FORWARD
                    if (next != null)
                    {
                        next.RequestFocus();
                        next.SetSelection(0);
                    }
                }
            }
        }

        private void InputView_TextChanged(object? sender, global::Android.Text.TextChangedEventArgs e)
        {
            if (e!.Text.Count() > 1)
            {
                var currentFocusedView = (EditTextWithClearFocus)sender!;
                currentFocusedView.Text = e.Text.ElementAt(e.Start).ToString();
                if ((int)currentFocusedView.Tag == _inputsCount - 1)
                {
                    currentFocusedView.SetSelection(1);
                }
            }

            TextValue = string.Join("", _editTexts.Select(t => t!.Text!));
        }

        private void InputView_KeyPress(object? sender, KeyEventArgs e)
        {
            if (e!.Event!.Action == KeyEventActions.Down &&
                e.KeyCode == Keycode.Del)
            {
                if (sender is EditTextWithClearFocus editText &&
                    (int)editText.Tag != 0 &&
                    editText.SelectionEnd == 0)
                {
                    var prev = (EditText)editText.FocusSearch(FocusSearchDirection.Left)!; // or FOCUS_FORWARD
                    if (prev != null)
                    {
                        prev.RequestFocus();
                        if (prev!.Text!.Length > 0)
                        {
                            prev.SetSelection(1);
                        }
                    }
                }
            }

            e.Handled = false;
        }

        public bool OnPreDraw()
        {
            ViewTreeObserver!.RemoveOnPreDrawListener(this);

            var sizesChanged = false;
            while (Width < ((_inputWidth * _inputsCount) + (_inputInnerSapce * (_inputsCount - 1))))
            {
                sizesChanged = true;
                if (_inputInnerSapce == _minInnerSpace)
                {

                    if (_inputWidth == _minInputWidth)
                    {
                        break;
                    }
                    else
                    {
                        _inputWidth--;
                    }
                }
                else
                {
                    _inputInnerSapce--;

                }
            }

            if (sizesChanged)
            {
                _inputHeight = (_inputWidth * _defaultInputHeight) / _defaultInputWidth;
                _inputTextSize = (_inputWidth * _defaultInputTextSize) / _defaultInputWidth;
            }

            _editTexts.Clear();

            for (int i = 0; i < _inputsCount; i++)
            {
                var inputView = GetEditTextInput(i);
                AddView(inputView);

                if (i != 0)
                {
                    var lp = inputView.LayoutParameters as MarginLayoutParams;
                    lp!.MarginStart = _inputInnerSapce;
                    inputView.LayoutParameters = lp;

                }
            }

            Post(() => RequestLayout());

            return true;
        }
    }
}