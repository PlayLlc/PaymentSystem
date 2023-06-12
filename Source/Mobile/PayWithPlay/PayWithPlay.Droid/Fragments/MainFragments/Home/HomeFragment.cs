using Android.Graphics;
using Android.Views;
using AndroidX.Core.Content;
using AndroidX.Lifecycle;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.Home;

namespace PayWithPlay.Droid.Fragments.MainFragments.Home
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(HomeViewModel))]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        private PieChart? _terminalsPieChart;

        public override int LayoutId => Resource.Layout.fragment_home;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnlineTerminalsAction = SetTerminalsChartData; 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root =  base.OnCreateView(inflater, container, savedInstanceState);

            SetTerminalsPieChart(root);

            return root;
        }

        private void SetTerminalsPieChart(View root)
        {
            _terminalsPieChart = root.FindViewById<PieChart>(Resource.Id.terminals_pie_chart)!;
            _terminalsPieChart.SetPadding(0, 0, 0, 0);
            _terminalsPieChart.SetExtraOffsets(0, 0, 0, 0);
            _terminalsPieChart.Description.Enabled = false;
            _terminalsPieChart.SetDrawCenterText(false);
            _terminalsPieChart.Legend.Enabled = false;
            _terminalsPieChart.SetDrawMarkers(false);
            _terminalsPieChart.SetDrawEntryLabels(false);
            _terminalsPieChart.SetDrawSlicesUnderHole(false);
            _terminalsPieChart.SetTransparentCircleColor(Color.Transparent);

            _terminalsPieChart.HoleRadius = 90;
            _terminalsPieChart.SetHoleColor(Color.Transparent);

            SetTerminalsChartData();
        }

        private void SetTerminalsChartData()
        {
            var entries = new List<PieEntry>
            {
                new PieEntry(ViewModel.OnlineTerminals),
                new PieEntry(ViewModel.TotalTerminals - ViewModel.OnlineTerminals),
            };

            if (_terminalsPieChart!.Data is PieData pieData &&
                pieData.DataSets is IList<PieDataSet> dataSets &&
                dataSets.FirstOrDefault() is PieDataSet currentLineDataSet) 
            {
                currentLineDataSet.Clear();
                currentLineDataSet.Values = entries;

                _terminalsPieChart.Data.NotifyDataChanged();
                _terminalsPieChart.NotifyDataSetChanged();
            }
            else
            {
                var pieDataSet = new PieDataSet(entries, null);
                pieDataSet.Color = Color.Transparent;
                pieDataSet.SetColors(new[] { ContextCompat.GetColor(Context, Resource.Color.accent_color), ContextCompat.GetColor(Context, Resource.Color.negative_color) });
                pieDataSet.SliceSpace = 0f;
                pieDataSet.HighlightEnabled = false;
                pieDataSet.SetDrawValues(false);

                _terminalsPieChart.Data = new PieData(pieDataSet);
            }
        }
    }
}
