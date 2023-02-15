using Android.Views;
using AndroidX.RecyclerView.Widget;
using PayWithPlay.Android.ViewHolders;
using PayWithPlay.Core.Models;

namespace PayWithPlay.Android.Adapters
{
    public class DeviceSettingsRecyclerAdapter : BaseRecyclerAdapter<DeviceSettingsItemModel>
    {
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ((DeviceSettingsViewHolder)holder).Bind(Items![position]);
        }

        public override RecyclerView.ViewHolder CreateItemViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)!.Inflate(Resource.Layout.row_device_settings, parent, false);
            return new DeviceSettingsViewHolder(itemView);
        }
    }
}