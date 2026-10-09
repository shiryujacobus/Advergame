using UnityEngine;
using UnityEngine.Audio;

public class SFXVolumeApplier : MonoBehaviour
{
    public AudioMixer audioMixer;

    private const string VolumeParameter = "SFXVolume";
    private const string PrefKey = "SFXVolume";

    void Start()
    {
        float volume = PlayerPrefs.GetFloat(PrefKey, 1f);

        float db = volume <= 0.0001f
            ? -80f
            : Mathf.Log10(volume) * 20f;

        audioMixer.SetFloat(VolumeParameter, db);
    }
}