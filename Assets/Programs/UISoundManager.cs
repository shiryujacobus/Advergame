
using UnityEngine;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioClip clickSound;

    public void PlayClickSound()
    {
        if (sfxAudioSource != null && clickSound != null)
        {
            sfxAudioSource.PlayOneShot(clickSound);
        }
    }
}