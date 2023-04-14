using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Enums;

namespace PayWithPlay.Droid.CustomBindings.Components
{
    public class ComponentTypeBinding : MvxAndroidTargetBinding<MaterialButton, ComponentType>
    {
        public const string Property = "ComponentType";

        public ComponentTypeBinding(MaterialButton target) : base(target)
        {
        }

        protected override void SetValueImpl(MaterialButton target, ComponentType value)
        {
            if (target == null) 
            {
                return;
            }

            switch (value)
            {
                case ComponentType.Add:
                    target.SetIconResource(Resource.Drawable.ic_plus);
                    break;
                case ComponentType.TeamManagement:
                    target.SetIconResource(Resource.Drawable.ic_team);
                    break;
                case ComponentType.Chat:
                    target.SetIconResource(Resource.Drawable.ic_chat);
                    break;
                case ComponentType.Invoices:
                    target.SetIconResource(Resource.Drawable.ic_invoices);
                    break;
            }
        }
    }
}
