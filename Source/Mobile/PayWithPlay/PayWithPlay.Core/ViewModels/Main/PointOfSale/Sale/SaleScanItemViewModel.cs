
namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleScanItemViewModel : BaseViewModel<Action<string>>
    {
        private Action<string>? _onScanResultAction;
        private bool _isLoading;

        public SaleScanItemViewModel()
        {
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _onScanResultAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public Action? OnNewScanAction { get; set; }

        public void OnScanResult(string result)
        {
            Task.Run(async () => 
            {
                IsLoading = true;

                await Task.Delay(2000);

                _onScanResultAction?.Invoke(result);

                IsLoading = false;

                OnNewScanAction?.Invoke();
            });
        }

        public override void Prepare(Action<string> parameter)
        {
            _onScanResultAction = parameter;
        }
    }
}
