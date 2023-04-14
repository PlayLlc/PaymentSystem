using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Media;
using MvvmCross.ViewModels;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels
{
    public class AddImageViewModel : BottomSheetItemsViewModel<Action<string?>>
    {
        private readonly MvxObservableCollection<BottomSheetItemModel> _items;
        private Action<string?>? _returnUrlAction;

        public AddImageViewModel()
        {
            _items = new MvxObservableCollection<BottomSheetItemModel>()
            {
                new BottomSheetItemModel
                {
                    Title = Resource.Camera,
                    IconType = Enums.BottomSheetIconType.Camera
                },
                new BottomSheetItemModel
                {
                    Title = Resource.PhotoLibrary,
                    IconType = Enums.BottomSheetIconType.PhotoLibrary
                },
            };
        }

        public override string Title => Resource.AddImage;

        public override MvxObservableCollection<BottomSheetItemModel> Items => _items;

        public override async void OnItem(BottomSheetItemModel item)
        {
            _ = NavigationService.Close(this);
            if (item.IconType == Enums.BottomSheetIconType.Camera)
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result == null)
                {
                    return;
                }
                _returnUrlAction?.Invoke(result?.FullPath);
            }
            else
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result == null)
                {
                    return;
                }
                _returnUrlAction?.Invoke(result?.FullPath);
            }

            _returnUrlAction = null;
        }

        public override void Prepare(Action<string?> action)
        {
            _returnUrlAction = action;
        }
    }
}
