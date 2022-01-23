using System.Text;
using Android.Nfc;
using Android.Nfc.Tech;

namespace AndroidPcd;

internal class PpseControl
{
    private static readonly byte[] _SelectPpse =
    {
        0x00, 0xA4, 0x04, 0x00, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31
    };

    public static byte[] _SelectApplication1 = {0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10};
    public static byte[] _SelectApplication2 = {0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40};
    private readonly TechFactory _TechFactory;
    private readonly TechTransceiver _TechTransceiver;
    private readonly Activity _Activity;
    private readonly TextView content_main;


    public PpseControl(Activity activity, TextView textView)
    {
        _Activity = activity;
        content_main = textView;
        _TechFactory = new TechFactory();
        _TechTransceiver = new TechTransceiver();
    }

    public byte[] Main(Tag tag)
    {
        BasicTagTechnology tech = _TechFactory.GetTech(tag);
        tech.Connect();
        byte[] ppseRApdu = GetPpse(tech);
        byte[] selectApplication1RApdu = GetApplication1(tech);


        return selectApplication1RApdu;
    }

    private byte[] GetPpse(BasicTagTechnology tech)
    {
        try
        {
            return _TechTransceiver.Transceive(tech, _SelectPpse)!;
        }
        catch (ArgumentOutOfRangeException exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\n\\r");
            sb.Append("======================================================");
            sb.Append($"EXCEPTION: {nameof(GetPpse)}");
            sb.Append(exception.Message);
            sb.Append("======================================================");
            _Activity.RunOnUiThread(() => { content_main!.Text = sb.ToString(); });
            throw;
        }
        catch (TagLostException exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\n\\r");
            sb.Append("======================================================");
            sb.Append($"EXCEPTION: {nameof(GetPpse)}");
            sb.Append(exception.Message);
            sb.Append("======================================================");
            _Activity.RunOnUiThread(() => { content_main!.Text = sb.ToString(); });
            throw;
        }
        catch (IOException exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\n\\r");
            sb.Append("======================================================");
            sb.Append($"EXCEPTION: {nameof(GetPpse)}");
            sb.Append(exception.Message);
            sb.Append("======================================================");
            _Activity.RunOnUiThread(() => { content_main!.Text = sb.ToString(); });
            throw;
        }
    }

    private byte[] GetApplication1(BasicTagTechnology tech)
    {
        try
        {
            return _TechTransceiver.Transceive(tech, _SelectApplication2)!;
        }
        catch (ArgumentOutOfRangeException exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\n\\r");
            sb.Append("======================================================");
            sb.Append($"EXCEPTION: {nameof(GetApplication1)}");
            sb.Append(exception.Message);
            sb.Append("======================================================");
            _Activity.RunOnUiThread(() => { content_main!.Text = sb.ToString(); });
            throw;
        }
        catch (TagLostException exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\n\\r");
            sb.Append("======================================================");
            sb.Append($"EXCEPTION: {nameof(GetApplication1)}");
            sb.Append(exception.Message);
            sb.Append("======================================================");
            _Activity.RunOnUiThread(() => { content_main!.Text = sb.ToString(); });
            throw;
        }
        catch (IOException exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\n\\r");
            sb.Append("======================================================");
            sb.Append($"EXCEPTION: {nameof(GetApplication1)}");
            sb.Append(exception.Message);
            sb.Append("======================================================");
            _Activity.RunOnUiThread(() => { content_main!.Text = sb.ToString(); });
            throw;
        }
    }
}