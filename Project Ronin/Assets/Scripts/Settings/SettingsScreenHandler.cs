//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreenHandler : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button subtitleOnButton;
    [SerializeField] private Button subtitleOffButton;

    void Awake()
    {
        masterVolumeSlider.value = Settings.MasterVolume.Value;
        musicVolumeSlider.value = Settings.MusicVolume.Value;
        voiceVolumeSlider.value = Settings.VoiceVolume.Value;
        sfxVolumeSlider.value = Settings.SfxVolume.Value;
        if (Settings.Subtitles.Value)
            subtitleOffButton.interactable = false;
        else
            subtitleOnButton.interactable = false;
    }

    public void ChangeMasterVolume(float volume) => Settings.MasterVolume.Value = volume;
    public void ChangeMusicVolume(float volume) => Settings.MusicVolume.Value = volume;
    public void ChangeVoiceVolume(float volume) => Settings.VoiceVolume.Value = volume;
    public void ChangeSfxVolume(float volume) => Settings.SfxVolume.Value = volume;
    public void ChangeSubtitles(bool enabled) => Settings.Subtitles.Value = enabled;
    public void ChangePause(bool paused) => Settings.paused = paused;

    public void PauseTime() => Time.timeScale = 0f;
    public void UnPauseTime() => Time.timeScale = 1f;
}
