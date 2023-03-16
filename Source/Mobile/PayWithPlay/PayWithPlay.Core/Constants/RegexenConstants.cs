namespace PayWithPlay.Core.Constants
{
    public class RegexenConstants
    {
        public const string USER_NAME = @"^[\p{L} ,.'-`]*$";
        public const string PHONE_NUMBER = @"\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})";
        public const string EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string PASSWORD = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#*%$?^&()\\-`.+,/\""])[A-Za-z\d!@#*%$?^&()\\-`.+,/\""]+$";
    }
}