using UnityEngine;
using UnityEngine.UI;

public class InGameAudioSettings : MonoBehaviour
{
    [Header("In-Game Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource[] sfxSources;

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

    public void ToggleSound()
    {
        soundOn = !soundOn;

        foreach (AudioSource source in sfxSources)
        {
            if (source != null)
                source.mute = !soundOn;
        }

        soundButtonImage.sprite =
            soundOn ? soundOnIcon : soundOffIcon;
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        if (bgmSource != null)
            bgmSource.mute = !musicOn;

        musicButtonImage.sprite =
            musicOn ? musicOnIcon : musicOffIcon;
    }
}