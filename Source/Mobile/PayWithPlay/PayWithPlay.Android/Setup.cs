using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Plugin;
using PayWithPlay.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace PayWithPlay.Android
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

        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.MethodBinding.Plugin>();
            base.LoadPlugins(pluginManager);
        }
    }
}
