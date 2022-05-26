/// <summary>Globally accessible collection of <see cref="Setting{T}">.</summary>
public static class Settings
{
    // Audio
    public static Setting<float> MasterVolume = new FloatSetting("Master Volume", 1.0f);
    public static Setting<float> MusicVolume = new FloatSetting("Music Volume", 0.40f);
    public static Setting<float> SfxVolume = new FloatSetting("Sfx Volume", 0.50f);
    public static Setting<float> VoiceVolume = new FloatSetting("Voice Volume", 1.0f);
    public static Setting<float>[] AudioSettings = {MasterVolume, MusicVolume, SfxVolume, VoiceVolume};

    // Text
    public static Setting<bool> Subtitles = new BoolSetting("Subtitles", true);
    
    // Gameplay State
    public static event System.Action<bool> onPause;
    private static bool _paused = false;
    public static bool paused
    {
        get => _paused;
        set
        {
            _paused = value;
            onPause?.Invoke(value);
        }
    }
}
