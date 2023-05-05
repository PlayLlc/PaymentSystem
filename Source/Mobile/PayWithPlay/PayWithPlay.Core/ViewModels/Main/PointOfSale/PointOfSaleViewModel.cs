using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Return;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale
{
    public class PointOfSaleViewModel : BaseViewModel
    {
        public PointOfSaleViewModel()
        {
        }

        public string Title => Resource.PointOfSale;
        public string Subtitle => Resource.PointOfSaleSubtitle;
        public string SaleButtonText => Resource.Sale;
        public string ReturnButtonText => Resource.Return;

        public void OnSale()
        {
            NavigationService.Navigate<SaleChooseProductsViewModel>();
        }

        public void OnReturn()
        {
            NavigationService.Navigate<ReturnViewModel>();
        }
    }
}
