using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.Button;
using MvvmCross.Binding.Attributes;
using PayWithPlay.Core.Enums;
using PayWithPlay.Droid.Extensions;
using System.Windows.Input;

namespace PayWithPlay.Droid.CustomViews
{
    public class NumericKeybaordView : FrameLayout, ViewTreeObserver.IOnPreDrawListener
    {
        private static readonly int _minVerticalInnerSpace = 4f.ToPx();
        private static readonly int _minSize = 60f.ToPx();

        private static readonly int _defaultTextSize = 32;
        private static readonly int _defaultSize = 70f.ToPx();
        private static readonly int _defaultVerticalInnerSpace = 12f.ToPx();

        private int _textSize = _defaultTextSize;
        private int _size = _defaultSize;
        private int _verticalInnerSapce = _defaultVerticalInnerSpace;

        private LinearLayoutCompat? _rowOne;
        private LinearLayoutCompat? _rowTwo;
        private LinearLayoutCompat? _rowThree;
        private LinearLayoutCompat? _rowFour;

        private MaterialButton? _keyOne;
        private MaterialButton? _keyTwo;
        private MaterialButton? _keyThree;
        private MaterialButton? _keyFour;
        private MaterialButton? _keyFive;
        private MaterialButton? _keySix;
        private MaterialButton? _keySeven;
        private MaterialButton? _keyEight;
        private MaterialButton? _keyNine;
        private MaterialButton? _keyFingerprint;
        private MaterialButton? _keyZero;
        private MaterialButton? _keyRemove;

        private ICommand? _keyClick;

        #region ctors

        public NumericKeybaordView(Context context) : base(context)
        {
            Init();
        }

        public NumericKeybaordView(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init();
        }

        public NumericKeybaordView(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        protected NumericKeybaordView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        #endregion

        [MvxSetToNullAfterBinding]
        public ICommand? KeyClick
        {
            get => _keyClick;
            set
            {
                if (ReferenceEquals(_keyClick, value))
                    return;

                _keyClick = value;
            }
        }

        public Action<NumericKeyboardKeyType>? OnKeyAction { get; set; }

        public void SetFingerprintEnabled(bool enabled)
        {
            if (enabled)
            {
                _keyFingerprint!.Visibility = ViewStates.Visible;
                _keyFingerprint.Clickable = true;
            }
            else
            {
                _keyFingerprint!.Visibility = ViewStates.Invisible;
                _keyFingerprint.Clickable = false;
            }
        }

        private void Init()
        {
            Inflate(Context, Resource.Layout.view_numeric_keyboard, this);

            _rowOne = FindViewById<LinearLayoutCompat>(Resource.Id.row_one);
            _rowTwo = FindViewById<LinearLayoutCompat>(Resource.Id.row_two);
            _rowThree = FindViewById<LinearLayoutCompat>(Resource.Id.row_three);
            _rowFour = FindViewById<LinearLayoutCompat>(Resource.Id.row_four);

            _keyOne = FindViewById<MaterialButton>(Resource.Id.key_one);
            _keyTwo = FindViewById<MaterialButton>(Resource.Id.key_two);
            _keyThree = FindViewById<MaterialButton>(Resource.Id.key_three);
            _keyFour = FindViewById<MaterialButton>(Resource.Id.key_four);
            _keyFive = FindViewById<MaterialButton>(Resource.Id.key_five);
            _keySix = FindViewById<MaterialButton>(Resource.Id.key_six);
            _keySeven = FindViewById<MaterialButton>(Resource.Id.key_seven);
            _keyEight = FindViewById<MaterialButton>(Resource.Id.key_eight);
            _keyNine = FindViewById<MaterialButton>(Resource.Id.key_nine);
            _keyFingerprint = FindViewById<MaterialButton>(Resource.Id.key_fingerprint);
            _keyZero = FindViewById<MaterialButton>(Resource.Id.key_zero);
            _keyRemove = FindViewById<MaterialButton>(Resource.Id.key_remove);


            SetOnClickListener(_keyOne, _keyTwo, _keyThree, _keyFour, _keyFive, _keySix, _keySeven, _keyEight, _keyNine, _keyFingerprint, _keyZero, _keyRemove);

            ViewTreeObserver!.AddOnPreDrawListener(this);
        }

        private void SetOnClickListener(params MaterialButton[] materialButtons)
        {
            foreach (var button in materialButtons)
            {
                button.Click += OnButtonClick;
            }
        }

        private void OnButtonClick(object? sender, EventArgs e)
        {
            var button = (MaterialButton)sender!;

            NumericKeyboardKeyType? keyType = button.Id switch
            {
                Resource.Id.key_one => NumericKeyboardKeyType.One,
                Resource.Id.key_two => NumericKeyboardKeyType.Two,
                Resource.Id.key_three => NumericKeyboardKeyType.Three,
                Resource.Id.key_four => NumericKeyboardKeyType.Four,
                Resource.Id.key_five => NumericKeyboardKeyType.Five,
                Resource.Id.key_six => NumericKeyboardKeyType.Six,
                Resource.Id.key_seven => NumericKeyboardKeyType.Seven,
                Resource.Id.key_eight => NumericKeyboardKeyType.Eight,
                Resource.Id.key_nine => NumericKeyboardKeyType.Nine,
                Resource.Id.key_fingerprint => NumericKeyboardKeyType.FingerPrint,
                Resource.Id.key_zero => NumericKeyboardKeyType.Zero,
                Resource.Id.key_remove => NumericKeyboardKeyType.Remove,
                _ => null
            };

            if(KeyClick?.CanExecute(keyType) == true && keyType != null)
            {
                KeyClick.Execute(keyType);
            }
        }

        public bool OnPreDraw()
        {
            ViewTreeObserver!.RemoveOnPreDrawListener(this);

            var parentHeight = ((ViewGroup)Parent!).Height;

            var sizesChanged = false;
            while (parentHeight < ((_size * 4) + (_verticalInnerSapce * 4)))
            {
                sizesChanged = true;
                if (_size == _minSize)
                {
                    break;
                }
                else
                {
                    _size--;
                    _verticalInnerSapce = (_size * _defaultVerticalInnerSpace) / _defaultSize;
                }
            }

            if (sizesChanged)
            {
                _textSize = (_size * _defaultTextSize) / _defaultSize;
                _verticalInnerSapce = Math.Max(_minVerticalInnerSpace, _verticalInnerSapce);

                SetVerticalInnerSpace(_rowOne, _rowTwo, _rowThree, _rowFour);
                SetButtonsSize(_keyOne, _keyTwo, _keyThree, _keyFour, _keyFive, _keySix, _keySeven, _keyEight, _keyNine, _keyFingerprint, _keyZero, _keyRemove);
                Post(() => RequestLayout());
            }
            else
            {
                SetButtonIcon(_keyFingerprint);
                SetButtonIcon(_keyRemove);
            }

            return true;
        }

        private void SetVerticalInnerSpace(params LinearLayoutCompat[] rows)
        {
            foreach (var row in rows)
            {
                var lp = row.LayoutParameters as MarginLayoutParams;
                lp!.BottomMargin = _verticalInnerSapce;
                row.LayoutParameters = lp;
            }
        }

        private void SetButtonsSize(params MaterialButton[] buttons)
        {
            foreach (var button in buttons)
            {
                button.SetHeight(_size);
                button.SetWidth(_size);
                button.CornerRadius = _size / 2;
                button.SetTextSize(ComplexUnitType.Sp, _textSize);

                var lp = button.LayoutParameters as MarginLayoutParams;
                lp!.Height = lp.Width = _size;
                button.LayoutParameters = lp;

                if (button.Id == Resource.Id.key_fingerprint ||
                   button.Id == Resource.Id.key_remove)
                {
                    SetButtonIcon(button);
                }
            }
        }

        private void SetButtonIcon(MaterialButton button)
        {
            var currentIconSize = Math.Max(button!.Icon!.IntrinsicHeight, button.Icon.IntrinsicWidth);
            var iconSize = (_size * currentIconSize) / _defaultSize;
            button.IconSize = iconSize;
        }
    }
}