using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using AndroidX.Core.Content;
using AndroidX.Core.Content.Resources;
using Google.Android.Material.Button;
using MvvmCross.Binding.Attributes;
using PayWithPlay.Core.Models;
using PayWithPlay.Droid.Extensions;
using System.ComponentModel;

namespace PayWithPlay.Droid.CustomViews
{
    public class RadioButtonsView : FrameLayout, INotifyPropertyChanged
    {
        private readonly int _containerPadding = 2.5f.ToPx();
        private readonly List<Button> _buttons = new List<Button>();

        private int _buttonsHeight = -1;
        private float _buttonsTextSize = -1;

        private Color _selectedTextColor;
        private Color _unSelectedTextColor;
        private View? _buttonBackground;
        private LinearLayout? _buttonsContainer;

        private TextView? _selectedButton;
        private int _selectedButtonPosition = -1;
        private bool _isAnimating;

        private List<RadioButtonModel>? _buttonModels;
        private int? _selectedButtonType;

        #region ctors

        public RadioButtonsView(Context context) : base(context)
        {
            Init();
        }

        public RadioButtonsView(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
            SetValuesFromAttrs(attrs);
            Init();
        }

        public RadioButtonsView(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            SetValuesFromAttrs(attrs);
            Init();
        }

        public RadioButtonsView(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            SetValuesFromAttrs(attrs);
            Init();
        }

        protected RadioButtonsView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        [MvxSetToNullAfterBinding]
        public List<RadioButtonModel>? ItemsSource
        {
            get => _buttonModels;
            set
            {
                if (value == _buttonModels)
                {
                    return;
                }

                _buttonModels = value;

                ResetSelectedButton();

                if (_buttonModels == null || _buttonModels.Count == 0)
                {
                    _buttonsContainer!.RemoveAllViews();
                    _buttons.Clear();

                    return;
                }

                if (_buttonModels.Count > _buttons.Count)
                {
                    for (var i = _buttons.Count; i < _buttonModels.Count; i++)
                    {
                        var button = GetNewButton();

                        _buttonsContainer!.AddView(button);
                        _buttons.Add(button);
                    }
                }
                else if (_buttonModels.Count < _buttons.Count)
                {
                    var endCount = _buttons.Count - _buttonModels.Count;
                    for (var i = 0; i < endCount; i++)
                    {
                        var buttonToBeRemoved = _buttons[^1];
                        buttonToBeRemoved.Click -= OnButtonClick;

                        _buttons.Remove(buttonToBeRemoved);
                        _buttonsContainer!.RemoveViewAt(_buttonsContainer.ChildCount - 1);
                        if (_buttonsContainer.ChildCount > 0)
                        {
                            _buttonsContainer.RemoveViewAt(_buttonsContainer.ChildCount - 1);
                        }
                    }
                }

                for (var i = 0; i < _buttons.Count; i++)
                {
                    var button = _buttons[i];

                    button.Tag = i;
                    button.Text = _buttonModels[i].Title;
                    button.Click -= OnButtonClick;
                    button.Click += OnButtonClick;
                }
            }
        }

        public int? SelectedButtonType 
        {
            get => _selectedButtonType;
            set
            {
                if (Equals(_selectedButtonType, value)) 
                {
                    return;
                }

                _selectedButtonType = value;
                

                if (_buttonModels?.Any() != true || _selectedButtonType == null)
                {
                    ResetSelectedButton();
                    return;
                }

                var position = _buttonModels.FindIndex(t => t.Type == _selectedButtonType);
                if (position == -1)
                {
                    ResetSelectedButton();
                }
                else
                {
                    SelectButtonByPosition(position);
                }
            }
        }

        private void OnButtonClick(object? sender, EventArgs e)
        {
            var button = (TextView)sender!;
            SelectButtonByPosition((int)button.Tag, true);
        }

        private void SelectButtonByPosition(int position, bool shouldAnimate = false)
        {
            if (position < 0 || position >= _buttons.Count)
            {
                ResetSelectedButton();
                return;
            }

            if (position == _selectedButtonPosition || _isAnimating)
            {
                return;
            }

            var button = _buttons[position];

            if (shouldAnimate && _selectedButton != null && _selectedButtonPosition != -1)
            {
                AnimateButtonBackground(button, position);
                return;
            }

            UnselectSelectedButton();

            _selectedButton = button;
            _selectedButtonPosition = position;

            RaiseButtonTypeChanged(_buttonModels![position]);

            _selectedButton.SetTextColor(_selectedTextColor);

            var color = GetColorRes(_buttonModels[position].Color);
            if (_selectedButton.Background is GradientDrawable backgroundDrawable &&
                (backgroundDrawable.Color == null || backgroundDrawable.Color.DefaultColor != color))
            {
                backgroundDrawable.SetColor(color);
            }
        }

        private void RaiseButtonTypeChanged(RadioButtonModel radioButtonModel)
        {
            _selectedButtonType = radioButtonModel.Type;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedButtonType)));
        }

        private void AnimateButtonBackground(TextView buttonTo, int buttonPosition)
        {
            _isAnimating = true;

            var currentSelectedButton = _selectedButton;
            var currentSelectedPosition = _selectedButtonPosition;

            _buttonBackground!.TranslationX = currentSelectedButton!.GetX();
            var buttonBackgroundLp = _buttonBackground.LayoutParameters!;
            buttonBackgroundLp.Width = currentSelectedButton.Width;
            _buttonBackground.LayoutParameters = buttonBackgroundLp;

            var selectedButtonTextColorAnimation = ValueAnimator.OfObject(new ArgbEvaluator(), _selectedTextColor.ToArgb(), _unSelectedTextColor.ToArgb());
            var unselectedButtonTextColorAnimation = ValueAnimator.OfObject(new ArgbEvaluator(), _unSelectedTextColor.ToArgb(), _selectedTextColor.ToArgb());
            var backgroundColorAnimation = ValueAnimator.OfObject(new ArgbEvaluator(), GetColorRes(_buttonModels![currentSelectedPosition].Color), GetColorRes(_buttonModels[buttonPosition].Color))!;
            var widthAnimation = ValueAnimator.OfInt(_buttonBackground.LayoutParameters.Width, buttonTo.Width)!;
            var translateAnimation = ObjectAnimator.OfFloat(_buttonBackground, "translationX", _buttonBackground.TranslationX, buttonTo.GetX());

            selectedButtonTextColorAnimation!.Update += (s, e) =>
            {
                currentSelectedButton.SetTextColor(new Color((int)selectedButtonTextColorAnimation.AnimatedValue));
            };

            unselectedButtonTextColorAnimation!.Update += (s, e) =>
            {
                buttonTo.SetTextColor(new Color((int)unselectedButtonTextColorAnimation.AnimatedValue));
            };

            var drawable = (GradientDrawable)_buttonBackground.Background!;
            backgroundColorAnimation.Update += (s, e) =>
            {
                drawable.SetColor((int)backgroundColorAnimation.AnimatedValue);
            };

            widthAnimation.Update += (s, e) =>
            {
                var buttonBackgroundLp = _buttonBackground.LayoutParameters;
                buttonBackgroundLp.Width = (int)widthAnimation.AnimatedValue;
                _buttonBackground.LayoutParameters = buttonBackgroundLp;
                _buttonBackground.RequestLayout();
            };

            var animatorSet = new AnimatorSet();
            animatorSet.SetInterpolator(new AccelerateDecelerateInterpolator());
            animatorSet.PlayTogether(selectedButtonTextColorAnimation, unselectedButtonTextColorAnimation, backgroundColorAnimation, widthAnimation, translateAnimation);
            animatorSet.SetDuration(300);

            animatorSet.AnimationStart += (s, e) =>
            {
                ((GradientDrawable)currentSelectedButton.Background!).SetColor(Color.Transparent);
                _buttonBackground.Visibility = ViewStates.Visible;
            };

            animatorSet.AnimationEnd += (s, e) =>
            {
                ((GradientDrawable)buttonTo.Background!).SetColor(GetColorRes(_buttonModels[buttonPosition].Color));
                _buttonBackground.Visibility = ViewStates.Gone;

                _isAnimating = false;
            };

            animatorSet.Start();

            _selectedButton = buttonTo;
            _selectedButtonPosition = buttonPosition;

            RaiseButtonTypeChanged(_buttonModels![buttonPosition]);
        }

        private int GetColorRes(RadioButtonModel.ColorType colorType)
        {
            return colorType switch
            {
                RadioButtonModel.ColorType.LightBlue => ContextCompat.GetColor(Context, Resource.Color.accent_color),
                _ => Color.Transparent,
            };
        }

        private void ResetSelectedButton()
        {
            UnselectSelectedButton();

            _selectedButtonType = null;
            _selectedButton = null;
            _selectedButtonPosition = -1;
        }

        private void UnselectSelectedButton()
        {
            if (_selectedButton != null)
            {
                if (_selectedButton.TextColors == null || _selectedButton.TextColors.DefaultColor != _unSelectedTextColor)
                {
                    _selectedButton.SetTextColor(_unSelectedTextColor);
                }

                if (_selectedButton.Background is GradientDrawable selectedButtonBackground
                    && (selectedButtonBackground.Color == null || selectedButtonBackground.Color.DefaultColor != Color.Transparent))
                {
                    selectedButtonBackground.SetColor(Color.Transparent);
                }
            }
        }

        private Button GetNewButton()
        {
            var button = new Button(Context)
            {
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, _buttonsHeight)
                {
                    Gravity = GravityFlags.CenterVertical
                }
            };
            button.SetMinWidth(0);
            button.SetMinimumWidth(0);
            button.SetMinHeight(0);
            button.SetMinimumHeight(0);
            button.Background = GetButtonDrawable();
            var buttonPadding = (int)(0.45 * _buttonsHeight);
            button.SetPadding(buttonPadding, 0, buttonPadding, 0);
            button.Gravity = GravityFlags.Center;
            button.SetAllCaps(false);
            button.SetTextColor(_unSelectedTextColor);
            button.SetIncludeFontPadding(false);
            button.Typeface = ResourcesCompat.GetFont(Context, Resource.Font.poppins_semibold);
            button.SetTextSize(ComplexUnitType.Px, _buttonsTextSize);

            return button;
        }

        private GradientDrawable GetButtonDrawable()
        {
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetCornerRadius(_buttonsHeight / 2f);
            gradientDrawable.SetColor(Color.Transparent);
            return gradientDrawable;
        }

        private void Init()
        {
            var padding = 2.5f.ToPx();
            SetPadding(padding, padding, padding, padding);
            SetClipChildren(false);
            SetClipToPadding(false);
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetCornerRadius((_buttonsHeight + 2 * padding) / 2f);
            gradientDrawable.SetColor(ContextCompat.GetColor(Context, Resource.Color.third_color));
            Background = gradientDrawable;

            _selectedTextColor = new Color(ContextCompat.GetColor(Context, Resource.Color.white));
            _unSelectedTextColor = new Color(ContextCompat.GetColor(Context, Resource.Color.primary_text_color));

            _buttonBackground = new Button(Context)
            {
                LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, _buttonsHeight)
                {
                    Gravity = GravityFlags.CenterVertical
                },
                Visibility = ViewStates.Gone,
                Background = GetButtonDrawable()
            };
            AddView(_buttonBackground);

            _buttonsContainer = new LinearLayout(Context)
            {
                LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, _buttonsHeight)
                {
                    Gravity = GravityFlags.CenterVertical
                },
                Orientation = Orientation.Horizontal,
                Elevation = 8f
            };
            _buttonsContainer.SetClipChildren(false);
            _buttonsContainer.SetClipToPadding(false);
            AddView(_buttonsContainer);
        }

        private void SetValuesFromAttrs(IAttributeSet attributeSet)
        {
            var attrs = Context!.Theme!.ObtainStyledAttributes(attributeSet, Resource.Styleable.RadioButtons, 0, 0);
            try
            {
                _buttonsHeight = attrs.GetDimensionPixelSize(Resource.Styleable.RadioButtons_buttonsHeight, -1);
                _buttonsTextSize = attrs.GetDimension(Resource.Styleable.RadioButtons_buttonsTextSize, 16f);
            }
            finally
            {
                attrs.Recycle();
            }

            if (_buttonsHeight == -1)
            {
                var parentHeight = GetViewHeightFromAttributes(attributeSet);
                if (parentHeight <= 0)
                {
                    _buttonsHeight = 25f.ToPx();
                }
                else
                {
                    _buttonsHeight = parentHeight - (2 * _containerPadding);
                }
            }
        }

        private int GetViewHeightFromAttributes(IAttributeSet attrSet)
        {
            var layoutHeight = attrSet.GetAttributeValue("http://schemas.android.com/apk/res/android", "layout_height");
            if (string.IsNullOrEmpty(layoutHeight))
            {
                return -1;
            }

            if (layoutHeight.Equals(ViewGroup.LayoutParams.MatchParent.ToString()))
            {
                return ViewGroup.LayoutParams.MatchParent;
            }
            else if (layoutHeight.Equals(ViewGroup.LayoutParams.WrapContent.ToString()))
            {
                return ViewGroup.LayoutParams.WrapContent;
            }

            var heightAttribute = Context!.Theme!.ObtainStyledAttributes(attrSet, new[] { global::Android.Resource.Attribute.LayoutHeight }, 0, 0);
            var height = -1;
            try
            {
                height = heightAttribute.GetDimensionPixelSize(0, -1);
            }
            finally
            {
                heightAttribute.Recycle();
            }

            return height;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) 
            {
                if(_buttons != null && _buttons.Any())
                {
                    foreach (var button in _buttons)
                    {
                        button.Click -= OnButtonClick;
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}
