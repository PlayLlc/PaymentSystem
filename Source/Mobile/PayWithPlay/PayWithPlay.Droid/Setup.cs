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
using PayWithPlay.Droid.CustomBindings.NumericKeybaordView;
using Android.Views;
using PayWithPlay.Droid.CustomBindings.CreateAccountBindings;
using PayWithPlay.Droid.CustomBindings.Components;
using PayWithPlay.Droid.CustomBindings.Inventory.CreateInventoryItem;
using PayWithPlay.Droid.CustomBindings.Inventory;
using Google.Android.Material.Chip;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.IoC;
using MvvmCross;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Droid.Utils;
using PayWithPlay.Droid.CustomBindings.PointOfSale;
using MvvmCross.Platforms.Android.Views;
using Android.Content;

namespace PayWithPlay.Droid
{
    public class Setup : MvxAndroidSetup<CoreApp>
    {
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);

            Mvx.IoCProvider?.LazyConstructAndRegisterSingleton<IWheelPicker>(() => new WheelPicker());
        }

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
            registry.RegisterCustomBindingFactory<MaterialButton>(ButtonWithIconCenterTextBinding.Property,
                (button) => new ButtonWithIconCenterTextBinding(button));
            registry.RegisterCustomBindingFactory<MaterialButton>(ComponentTypeBinding.Property,
                (button) => new ComponentTypeBinding(button));

            // Images
            registry.RegisterCustomBindingFactory<AppCompatImageView>(VerifyIdentityImageBinding.Property,
                (imageView) => new VerifyIdentityImageBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(MerchantTypeBinding.Property,
                (imageView) => new MerchantTypeBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(ArrowIndicatorAnimationBinding.Property,
                (imageView) => new ArrowIndicatorAnimationBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(GlideImageBinding.Property,
                (imageView) => new GlideImageBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(ProfilePictureBinding.Property,
                (imageView) => new ProfilePictureBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(BottomSheetIconTypeBinding.Property,
                (imageView) => new BottomSheetIconTypeBinding(imageView));

            // Views
            registry.RegisterCustomBindingFactory<View>(HandleNestedScrollBinding.Property,
                (view) => new HandleNestedScrollBinding(view));
            registry.RegisterCustomBindingFactory<View>(InventoryItemBackgroundBinding.Property,
                (view) => new InventoryItemBackgroundBinding(view));
            registry.RegisterCustomBindingFactory<InputBoxesView>(InputBoxesBinding.Property,
                (inputBoxes) => new InputBoxesBinding(inputBoxes));

            registry.RegisterCustomBindingFactory<ProgressBar>(ProgressBarProgressBinding.Property,
                (progressBar) => new ProgressBarProgressBinding(progressBar));
            registry.RegisterCustomBindingFactory<ProgressBar>(ProgressBarMaxBinding.Property,
                (progressBar) => new ProgressBarMaxBinding(progressBar));
            registry.RegisterCustomBindingFactory<NumericKeybaordView>(NumericKeyboardFingerprintBinding.Property,
                (numericKeybaodView) => new NumericKeyboardFingerprintBinding(numericKeybaodView));
            registry.RegisterCustomBindingFactory<ChipGroup>(InventoryItemCategoriesBinding.Property,
                (categoriesView) => new InventoryItemCategoriesBinding(categoriesView));
            registry.RegisterCustomBindingFactory<RadioButtonsView>(RadioButtonsSelectionBinding.Property,
                (radioButtonsView) => new RadioButtonsSelectionBinding(radioButtonsView));

            registry.RegisterCustomBindingFactory<TextInputLayout>(SetErrorInputBinding.Property,
                (inputLayout) => new SetErrorInputBinding(inputLayout));

            // TextViews
            registry.RegisterCustomBindingFactory<TextView>(TextStyleBinding.Property,
                (textView) => new TextStyleBinding(textView));
            registry.RegisterCustomBindingFactory<TextView>(RedAsteriskBinding.Property,
                (textView) => new RedAsteriskBinding(textView));
            registry.RegisterCustomBindingFactory<TextView>(TransactionPriceTextBinding.Property,
                (textView) => new TransactionPriceTextBinding(textView));
            registry.RegisterCustomBindingFactory<TextView>(UnderlineTextBinding.Property,
                (textView) => new UnderlineTextBinding(textView));
            registry.RegisterCustomBindingFactory<TextView>(StrikethruTextBinding.Property,
                (textView) => new StrikethruTextBinding(textView));
            registry.RegisterCustomBindingFactory<TextView>(InventoryItemPriceTextColorBinding.Property,
                (textView) => new InventoryItemPriceTextColorBinding(textView));

            registry.RegisterCustomBindingFactory<View>(UserNameViewBinding.Property,
                (userNameView) => new UserNameViewBinding(userNameView));
            registry.RegisterCustomBindingFactory<View>(AddressViewBinding.Property,
                (addressView) => new AddressViewBinding(addressView));
            registry.RegisterCustomBindingFactory<View>(PhoneNumberViewBinding.Property,
                (phoneNumberView) => new PhoneNumberViewBinding(phoneNumberView));
            registry.RegisterCustomBindingFactory<View>(BusinessNameViewBinding.Property,
                (businessNameView) => new BusinessNameViewBinding(businessNameView));

            registry.RegisterCustomBindingFactory<View>(ItemViewBinding.Property,
                (itemView) => new ItemViewBinding(itemView));
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.MethodBinding.Plugin>();
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Visibility.Platforms.Android.Plugin>();
            base.LoadPlugins(pluginManager);
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new CustomViewPresenter(AndroidViewAssemblies);
        }

        protected override IMvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new CustomAndroidViewsContainer(applicationContext);
        }
    }
}
