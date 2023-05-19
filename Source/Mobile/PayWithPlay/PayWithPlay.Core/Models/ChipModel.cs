using PayWithPlay.Core.Enums;

namespace PayWithPlay.Core.Models
{
    public class ChipModel
    {
        public string? Title { get; set; }

        public long Id { get; set; }

        public ChipType Type { get; set; }
    }
}
