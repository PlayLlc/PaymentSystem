using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Android.ViewHolders
{
    public class DeviceSettingsViewHolder : RecyclerView.ViewHolder, ISafeDisposable
    {
        private DeviceSettingsItemModel? _model;

        public DeviceSettingsViewHolder(View itemView) : base(itemView)
        {
        }

        public void SafeDispose()
        {
            UnBind();
        }

        public void Bind(DeviceSettingsItemModel model)
        {
            UnBind();

            _model = model;

            Bind();
        }

        private void UnBind()
        {
        }

        private void Bind()
        {
            ((MaterialButton)ItemView).Text = _model.Title;
        }
    }
}
