using PayWithPlay.Core.ViewModels.Main.PointOfSale.Return;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleScanItemViewModel : BaseViewModel<Action<string>>
    {
        public SaleScanItemViewModel()
        {
        }

        public void OnScanResult(string result)
        {
        }

        public override void Prepare(Action<string> parameter)
        {
        }
    }
}
