using Tools;

public class AudioSignal : SignalData
{
    public readonly string ClipName;
    public readonly float Volume;

    public const string Bloop = "Bloop";
    public const string Swoosh = "Swoosh";
    public const string Button_Click = "Button_Click";
    public const string Tap = "Coin";

    public AudioSignal(string clipName, float volume = 1f)
    {
        ClipName = clipName;
        Volume = volume;
    }
}