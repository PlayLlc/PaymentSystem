using MvvmCross;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using PayWithPlay.Droid.CustomViews;
using System.Reflection;

namespace PayWithPlay.Droid
{
    public class CustomViewPresenter : MvxAndroidViewPresenter
    {
        public CustomViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            AttributeTypesToActionsDictionary.Register<MvxNavFragmentPresentationAttribute>(ShowNavFragment, CloseNavFragment);
        }

        private Task<bool> ShowNavFragment(Type view, MvxNavFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            ValidateArguments(view, attribute, request);

            if (CurrentActivity == null)
                throw new InvalidOperationException("CurrentActivity is null");

            if (CurrentFragmentManager == null)
                throw new InvalidOperationException("CurrentFragmentManager is null. Cannot create Fragment Transaction.");

            var navHostFragment = (MainNavHostFragment)CurrentFragmentManager.FindFragmentById(attribute.FragmentMainNavContainerId)! ?? throw new InvalidOperationException("Cannot find navigation fragment.");

             
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                var viewModelCache = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
                viewModelCache.Cache(instanceRequest.ViewModelInstance);
            }

            navHostFragment.NavController.Navigate(attribute.FragmentNavigationActionId);

            return Task.FromResult(true);
        }


        private Task<bool> CloseNavFragment(IMvxViewModel viewModel, MvxNavFragmentPresentationAttribute attribute)
        {
            ValidateArguments(attribute);

            if (CurrentFragmentManager == null)
                throw new InvalidOperationException("CurrentFragmentManager is null. Cannot find navigation fragment.");

            var navHostFragment = (MainNavHostFragment)CurrentFragmentManager.FindFragmentById(attribute.FragmentMainNavContainerId)! ?? throw new InvalidOperationException("Cannot find navigation fragment.");

            navHostFragment.NavController.NavigateUp();

            return Task.FromResult(true);
        }

        private static void ValidateArguments(Type? view, MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            ValidateArguments(attribute, request);
        }

        private static void ValidateArguments(MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            ValidateArguments(attribute);

            ValidateArguments(request);
        }

        private static void ValidateArguments(MvxBasePresentationAttribute? attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
        }

        private static void ValidateArguments(MvxViewModelRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
        }
    }
}
