
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    public Slider bgmVolumeSlider;

    void Start()
    {
        if (bgmAudioSource == null ||
            bgmVolumeSlider == null)
        {
            Debug.LogWarning(
                "AudioSource atau Slider belum dihubungkan!"
            );
            return;
        }

        bgmVolumeSlider.minValue = 0f;
        bgmVolumeSlider.maxValue = 1f;

        bgmVolumeSlider.value = bgmAudioSource.volume;

        bgmVolumeSlider.onValueChanged.AddListener(
            ChangeBGMVolume
        );
    }

    public void ChangeBGMVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }

    void OnDestroy()
    {
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.onValueChanged.RemoveListener(
                ChangeBGMVolume
            );
        }
    }
}