using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;

namespace PayWithPlay.Droid.TemplateSelectors
{
    public class CreateAccountStepTemplateSelector : IMvxTemplateSelector
    {
        private readonly Dictionary<Type, int> _itemsTypeDictionary = new()
        {
            [typeof(UserNameViewModel)] = Resource.Layout.fragment_user_registration_user_name,
            [typeof(UserPhoneNumberViewModel)] = Resource.Layout.fragment_user_registration_phone_number,
            [typeof(VerifyPhoneNumberViewModel)] = Resource.Layout.fragment_verify_identity,
            [typeof(UserAdressViewModel)] = Resource.Layout.fragment_user_adress
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