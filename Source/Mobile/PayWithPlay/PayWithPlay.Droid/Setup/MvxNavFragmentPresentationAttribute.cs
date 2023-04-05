using MvvmCross.Presenters.Attributes;

namespace PayWithPlay.Droid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxNavFragmentPresentationAttribute : MvxBasePresentationAttribute
    {
        public int FragmentNavigationActionId { get; set; }
        public int FragmentMainNavContainerId { get; set; }
    }
}
