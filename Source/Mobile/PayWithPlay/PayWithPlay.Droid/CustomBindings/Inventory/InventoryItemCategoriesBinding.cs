using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AndroidX.Core.Content;
using AndroidX.Core.Content.Resources;
using AndroidX.Core.View;
using Google.Android.Material.Chip;
using Java.Lang;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.CustomBindings.Inventory
{
    public class InventoryItemCategoriesBinding : MvxAndroidTargetBinding<ChipGroup, List<ChipModel>>
    {
        public const string Property = "ItemCategories";

        public InventoryItemCategoriesBinding(ChipGroup target) : base(target)
        {
        }

        protected override void SetValueImpl(ChipGroup target, List<ChipModel> categories)
        {
            if (target == null)
            {
                return;
            }

            target.RemoveAllViews();

            if (categories == null || categories.Count == 0)
            {
                return;
            }

            var values = categories.Where(t => !string.IsNullOrWhiteSpace(t.Title)).ToList();
            if (target.Parent is FrameLayout parent && parent.Width > 0)
            {
                SetChips(target, parent.Width, values);
            }
            else
            {
                ViewKt.DoOnPreDraw(target, new NativeAction((view) =>
                {
                    var chipGroup = (ChipGroup)view!;
                    var parent = (FrameLayout)chipGroup.Parent!;

                    SetChips(chipGroup, parent.Width, values);
                }));
            }
        }

        private void SetChips(ChipGroup chipGroup, int maxWidth, List<ChipModel> values)
        {
            if (values == null || values.Count == 0)
            {
                return;
            }
            var valuesCount = values.Count;
            var countChip = GetChip(chipGroup.Context, ChipType.ItemCategory);
            var chipsWidthSum = 0;

            for (int i = 0; i < valuesCount; i++)
            {
                var moreChipsChipWidth = 0;
                if (valuesCount - i + 1 > 0)
                {
                    countChip.Text = $"+{valuesCount - i}";
                    countChip.Measure(0, 0);
                    moreChipsChipWidth = countChip.MeasuredWidth;
                }

                var chip = GetChip(chipGroup.Context, values[i].Type);
                chip.Text = values[i].Title;
                chip.Measure(0, 0);
                var chipWidth = chip.MeasuredWidth;

                if (chipsWidthSum + chipWidth + chipGroup.ChipSpacingHorizontal + moreChipsChipWidth > maxWidth)
                {
                    countChip.Text = $"+{valuesCount - i}";
                    chipGroup.AddView(countChip);
                    break;
                }

                chipGroup.AddView(chip);
                chipsWidthSum += chipWidth + chipGroup.ChipSpacingHorizontal;
            }
        }

        private Chip GetChip(Context context, ChipType chipType)
        {
            var chip = new Chip(context);
            chip.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, 16f.ToPx());
            chip.SetHeight(16f.ToPx());
            chip.SetSingleLine(true);
            chip.SetLines(1);
            chip.SetEnsureMinTouchTargetSize(false);
            chip.SetPadding(0, 0, 0, 0);
            chip.SetTextSize(ComplexUnitType.Sp, 10);
            chip.TextStartPadding = 8f.ToPx();
            chip.TextEndPadding = 8f.ToPx();
            chip.ChipStartPadding = 0;
            chip.ChipEndPadding = 0;
            chip.ChipStrokeWidth = 0;
            chip.Typeface = ResourcesCompat.GetFont(context, Resource.Font.poppins_semibold);
            chip.ChipCornerRadius = 8f.ToPx();

            if (chipType == ChipType.ItemDiscount)
            {
                chip.ChipBackgroundColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(context, Resource.Color.bright_gray)));
                chip.SetTextColor(ColorStateList.ValueOf(new Color(ContextCompat.GetColor(context, Resource.Color.accent_color))));
            }
            else 
            {
                chip.ChipBackgroundColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(context, Resource.Color.secondary_text_color)));
                chip.SetTextColor(ColorStateList.ValueOf(new Color(ContextCompat.GetColor(context, Resource.Color.black))));
            }

            return chip;
        }
    }
}
