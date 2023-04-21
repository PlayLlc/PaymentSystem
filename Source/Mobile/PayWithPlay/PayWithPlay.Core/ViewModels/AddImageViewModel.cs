using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
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
            FileResult? result = null;
            try
            {
                if (item.IconType == Enums.BottomSheetIconType.Camera)
                {

                    result = await MediaPicker.CapturePhotoAsync();
                }
                else
                {
                    result = await MediaPicker.PickPhotoAsync();
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException permissionException)
                {
                }
            }
            if (result == null)
            {
                return;
            }

            await Task.Delay(100);
            _ = NavigationService.Navigate<ConfirmPhotoViewModel, ConfirmPhotoViewModel.NavigationData>(new ConfirmPhotoViewModel.NavigationData
            {
                ImageUrl = result.FullPath,
                ReturnAction = _returnUrlAction
            });

            _returnUrlAction = null;
        }

        public override void Prepare(Action<string?> action)
        {
            _returnUrlAction = action;
        }
    }
}