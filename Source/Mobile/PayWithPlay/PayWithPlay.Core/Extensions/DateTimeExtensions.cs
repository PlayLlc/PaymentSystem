namespace PayWithPlay.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string DisplayFormat(this DateTime? dateTime)
        {
            if(dateTime == null)
            {
                return string.Empty;
            }

            return dateTime.Value.ToString("dd/MM/yyyy");
        }
    }
}
