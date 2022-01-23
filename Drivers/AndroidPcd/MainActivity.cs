using Android.Nfc;

namespace AndroidPcd;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity, NfcAdapter.IReaderCallback
{
    private static readonly byte[] _SelectApdu =
    {
        0x00, 0xA4, 0x04, 0x00, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31
    };

    public static NfcReaderFlags READER_FLAGS =
        NfcReaderFlags.NfcA | NfcReaderFlags.NfcB | NfcReaderFlags.NfcF | NfcReaderFlags.SkipNdefCheck;

    private Activity? _Activity;
    private NfcAdapter? _NfcAdapter;
    private PpseControl? _PpseControl;
    private TextView? content_main;

    public void OnTagDiscovered(Tag? tag)
    {
        if (tag == null)
            throw new ArgumentNullException(nameof(tag));

        byte[] application1 = _PpseControl!.Main(tag);

        string resultz = "";
        for (int i = 0; i < application1.Length; i++) resultz += $", 0x{application1[i]:X2}";

        Console.WriteLine(resultz);

        RunOnUiThread(() => { content_main!.Text = resultz; });
    }


    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_main);
        content_main = (TextView) FindViewById(Resource.Id.textView)!;
        _Activity = this;
        _PpseControl = new PpseControl(_Activity, content_main!);

        _NfcAdapter = NfcAdapter.GetDefaultAdapter(_Activity);

        if (_NfcAdapter == null)
        {
            Toast.MakeText(_Activity, "No PICC Detected", ToastLength.Short)!.Show();
            _Activity.Finish();
            return;
        }

        _NfcAdapter.EnableReaderMode(_Activity, this, READER_FLAGS, null);
    }
}