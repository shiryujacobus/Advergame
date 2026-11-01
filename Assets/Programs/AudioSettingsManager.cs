using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource bgmAudioSource;
    public AudioSource[] soundAudioSources;

    [Header("Button Images")]
    public Image soundButtonImage;
    public Image musicButtonImage;

    [Header("Sound Icons")]
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;

    [Header("Music Icons")]
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    private bool soundOn = true;
    private bool musicOn = true;

    void Start()
    {
        UpdateSound();
        UpdateMusic();
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
        UpdateSound();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        UpdateMusic();
    }

    void UpdateSound()
    {
        foreach (AudioSource source in soundAudioSources)
        {
            if (source != null)
                source.mute = !soundOn;
        }

        if (soundButtonImage != null)
        {
            soundButtonImage.sprite =
                soundOn ? soundOnIcon : soundOffIcon;
        }
    }

    void UpdateMusic()
    {
        if (bgmAudioSource != null)
            bgmAudioSource.mute = !musicOn;

        if (musicButtonImage != null)
        {
            musicButtonImage.sprite =
                musicOn ? musicOnIcon : musicOffIcon;
        }
    }
}