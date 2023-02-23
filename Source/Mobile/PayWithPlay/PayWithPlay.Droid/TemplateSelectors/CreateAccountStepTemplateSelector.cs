using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using PayWithPlay.Core.ViewModels.CreateAccount.MerchantRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;

namespace PayWithPlay.Droid.TemplateSelectors
{
    public class CreateAccountStepTemplateSelector : IMvxTemplateSelector
    {
        private readonly Dictionary<Type, int> _itemsTypeDictionary = new()
        {
            [typeof(UserNameViewModel)] = Resource.Layout.fragment_registration_user_name,
            [typeof(UserPhoneNumberViewModel)] = Resource.Layout.fragment_registration_user_phone_number,
            [typeof(VerifyPhoneNumberViewModel)] = Resource.Layout.fragment_verify_identity,
            [typeof(RegistrationAddressViewModel)] = Resource.Layout.fragment_registration_user_address,
            [typeof(HomeOrBusinessAddressViewModel)] = Resource.Layout.fragment_registration_home_or_business_address,
            [typeof(MerchantTypeViewModel)] = Resource.Layout.fragment_registration_merchant_type,
            [typeof(BusinessNameViewModel)] = Resource.Layout.fragment_registration_business_name
        };

        public int ItemTemplateId { get; set; }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            return _itemsTypeDictionary[forItemObject.GetType()];
        }
    }
}