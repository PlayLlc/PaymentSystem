using AndroidX.Lifecycle;

namespace PayWithPlay.Android.Lifecycle
{
    public static class ViewModelProviders
    {
        public static ViewModelProvider Of(IViewModelStoreOwner viewModelStoreOwner)
        {
            return new ViewModelProvider(viewModelStoreOwner, new ViewModelProvider.NewInstanceFactory());
        }
    }
}
