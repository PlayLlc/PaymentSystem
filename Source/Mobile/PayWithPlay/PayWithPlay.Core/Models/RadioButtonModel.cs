namespace PayWithPlay.Core.Models
{
    public class RadioButtonModel
    {
        public enum ColorType
        {
            LightBlue
        }

        public string? Title { get; set; }

        public int Type { get; set; }

        public ColorType Color { get; set; }
    }
}
