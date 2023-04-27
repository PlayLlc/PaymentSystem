namespace PayWithPlay.Core.ViewModels.Main.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
        }

        public string? UserPictureUrl { get; set; }
        public string? UserFullName { get; set; } = "Matt Jerred";
        public string? UserLocation { get; set; } = "Dublin, Ireland";

        public void OnNotifications()
        {
        }
    }
}