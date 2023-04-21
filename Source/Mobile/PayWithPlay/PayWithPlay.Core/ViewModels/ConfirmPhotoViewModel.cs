using PayWithPlay.Core.Resources;
using static PayWithPlay.Core.ViewModels.ConfirmPhotoViewModel;

namespace PayWithPlay.Core.ViewModels
{
    public class ConfirmPhotoViewModel : BaseViewModel<NavigationData>
    {
        public class NavigationData
        {
            public string? ImageUrl { get; set; }

            public Action<string>? ReturnAction { get; set; }
        }

        private volatile bool _onDonePressed;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            CropImageTask = null;
        }

        public string Title => Resource.ConfirmPhotoTitle;
        public string CancelButtonText => Resource.Cancel;
        public string DoneButtonText => Resource.Done;

        public NavigationData? Data { get; set; }
        public Func<Task>? CropImageTask { get; set; }

        public void OnBack()
        {
        }

        public void OnCancel()
        {
            NavigationService.Close(this);
        }

        public async void OnDone()
        {
            if (_onDonePressed)
            {
                return;
            }
            _onDonePressed = true;

            await CropImageTask!.Invoke();
            _= NavigationService.Close(this);
            Data!.ReturnAction?.Invoke(Data.ImageUrl);
            Data!.ReturnAction = null;
        }

        public override void Prepare(NavigationData parameter)
        {
            Data = parameter;
        }
    }
}
