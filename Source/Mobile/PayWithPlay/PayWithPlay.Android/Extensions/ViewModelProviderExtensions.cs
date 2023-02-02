using AndroidX.Lifecycle;
using PayWithPlay.Core.ViewModels;

namespace PayWithPlay.Android.Extensions
{
    public static class ViewModelProviderExtensions
    {
        public static T Get<T>(this ViewModelProvider viewModelProvier, Func<T?> viewModelFactory) where T : BaseViewModel
        {
            var androidViewModelHolder = (AndroidViewModelHolder)viewModelProvier.Get(Java.Lang.Class.FromType(typeof(AndroidViewModelHolder)));

            return androidViewModelHolder.GetViewModel(viewModelFactory);
        }

        // Cannot make the class generic due to: Pending exception android.runtime.JavaProxyThrowable: System.NotSupportedException: Constructing instances of generic types from Java is not supported, as the type parameters cannot be determined.
        private class AndroidViewModelHolder : ViewModel
        {
            private object? _viewModel;

            public T GetViewModel<T>(Func<T?> viewModelFactory)
                where T : BaseViewModel
            {
                _viewModel ??= viewModelFactory();

                return (T)_viewModel;
            }
        }
    }
}
