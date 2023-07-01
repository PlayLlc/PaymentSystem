namespace PayWithPlay.Core.Models
{
    public class RadioButtonModel
    {
        public enum ColorType
        {
            Transparent,
            LightBlue
        }

        public string? Title { get; set; }

        public int Type { get; set; }

        public ColorType Color { get; set; }
    }
}
