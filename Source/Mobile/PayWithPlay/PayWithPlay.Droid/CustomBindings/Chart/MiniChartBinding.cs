using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Models.Chart;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.CustomBindings.Chart
{
    public class MiniChartBinding : MvxAndroidTargetBinding<MiniChartView, MiniChartModel>
    {
        public const string Property = "MiniChartData";

        public MiniChartBinding(MiniChartView target) : base(target)
        {
        }

        protected override void SetValueImpl(MiniChartView target, MiniChartModel value)
        {
            if (target == null) 
            {
                return;
            }

            target.SetData(value.IsPositive, value.ValueDisplayed, value.Entries);
        }
    }
}
