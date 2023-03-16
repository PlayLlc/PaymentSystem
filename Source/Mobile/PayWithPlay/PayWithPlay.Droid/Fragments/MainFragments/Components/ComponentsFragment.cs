using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.Components;

namespace PayWithPlay.Droid.Fragments.MainFragments.Components
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_containerView, ViewModelType = typeof(ComponentsViewModel))]
    public class ComponentsFragment : BaseFragment<ComponentsViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_components;
    }
}