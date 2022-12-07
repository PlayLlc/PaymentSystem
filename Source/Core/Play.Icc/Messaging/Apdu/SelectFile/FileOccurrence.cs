namespace Play.Icc.Messaging.Apdu.SelectFile;

public enum FileOccurrence
{
    FirstOrOnly = 0x00,
    Last = 0x01,
    Next = 0x02,
    Previous = 0x03
}