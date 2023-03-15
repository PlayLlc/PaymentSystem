using AndroidX.AppCompat.Widget;
using Google.Android.Material.Button;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Plugin;
using PayWithPlay.Droid.CustomBindings;
using PayWithPlay.Core;
using Serilog;
using Serilog.Extensions.Logging;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.CustomBindings.IndicatorBindings;
using Google.Android.Material.TextField;
using AndroidX.Core.Widget;
using PayWithPlay.Droid.CustomBindings.NumericKeybaordView;
using Android.Views;
using PayWithPlay.Droid.CustomBindings.CreateAccountBindings;

namespace PayWithPlay.Droid
{
    public class Setup : MvxAndroidSetup<CoreApp>
    {
        protected override ILoggerFactory? CreateLogFactory()
        {
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override ILoggerProvider? CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            // Buttons
            registry.RegisterCustomBindingFactory<MaterialButton>(DeviceSettingButtonBinding.Property,
                (button) => new DeviceSettingButtonBinding(button));
            registry.RegisterCustomBindingFactory<MaterialButton>(UnderlineButtonTextBinding.Property,
                (button) => new UnderlineButtonTextBinding(button));

            // Images
            registry.RegisterCustomBindingFactory<AppCompatImageView>(VerifyIdentityImageBinding.Property,
                (imageView) => new VerifyIdentityImageBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(MerchantTypeBinding.Property,
                (imageView) => new MerchantTypeBinding(imageView));

            // Views
            registry.RegisterCustomBindingFactory<NestedScrollView>(ScrollViewHandleNestedScrollBinding.Property,
                (scrollView) => new ScrollViewHandleNestedScrollBinding(scrollView));
            registry.RegisterCustomBindingFactory<InputBoxesView>(InputBoxesBinding.Property,
                (inputBoxes) => new InputBoxesBinding(inputBoxes));

            registry.RegisterCustomBindingFactory<ProgressBar>(ProgressBarProgressBinding.Property,
                (progressBar) => new ProgressBarProgressBinding(progressBar));
            registry.RegisterCustomBindingFactory<ProgressBar>(ProgressBarMaxBinding.Property,
                (progressBar) => new ProgressBarMaxBinding(progressBar));
            registry.RegisterCustomBindingFactory<NumericKeybaordView>(NumericKeyboardFingerprintBinding.Property,
                (numericKeybaodView) => new NumericKeyboardFingerprintBinding(numericKeybaodView));

            registry.RegisterCustomBindingFactory<TextInputLayout>(SetErrorInputBinding.Property,
                (inputLayout) => new SetErrorInputBinding(inputLayout));

            // TextViews
            registry.RegisterCustomBindingFactory<TextView>(TextStyleBinding.Property,
                (textView) => new TextStyleBinding(textView));

            registry.RegisterCustomBindingFactory<View>(UserNameViewBinding.Property,
                (userNameView) => new UserNameViewBinding(userNameView));
            registry.RegisterCustomBindingFactory<View>(AddressViewBinding.Property,
                (userNameView) => new AddressViewBinding(userNameView));
            registry.RegisterCustomBindingFactory<View>(PhoneNumberViewBinding.Property,
                (userNameView) => new PhoneNumberViewBinding(userNameView));
            registry.RegisterCustomBindingFactory<View>(BusinessNameViewBinding.Property,
                (userNameView) => new BusinessNameViewBinding(userNameView));
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.MethodBinding.Plugin>();
            base.LoadPlugins(pluginManager);
        }
    }
}
