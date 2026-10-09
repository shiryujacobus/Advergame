using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SFXSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sfxSlider;

    private const string VolumeParameter = "SFXVolume";
    private const string PrefKey = "SFXVolume";

    void Start()
    {
        if (audioMixer == null || sfxSlider == null)
        {
            Debug.LogError("SFXSettings: AudioMixer atau Slider belum di-assign!");
            return;
        }

        float savedVolume = PlayerPrefs.GetFloat(PrefKey, 1f);

        sfxSlider.SetValueWithoutNotify(savedVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        ApplyVolume(savedVolume);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyVolume(PlayerPrefs.GetFloat(PrefKey, 1f));
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(PrefKey, volume);
        PlayerPrefs.Save();

        ApplyVolume(volume);
    }

    void ApplyVolume(float volume)
    {
        if (audioMixer == null)
        {
            Debug.LogError("SFXSettings: AudioMixer belum di-assign!");
            return;
        }

        float db = volume <= 0.0001f
            ? -80f
            : Mathf.Log10(volume) * 20f;

        bool success = audioMixer.SetFloat(VolumeParameter, db);

        Debug.Log(
            $"SFX DEBUG | Scene: {SceneManager.GetActiveScene().name}" +
            $" | Slider: {volume}" +
            $" | Target: {db} dB" +
            $" | SetFloat berhasil: {success}" +
            $" | Mixer: {audioMixer.name}"
        );

        if (!success)
        {
            Debug.LogError(
                $"Gagal mengatur '{VolumeParameter}'. " +
                "Periksa nama parameter dan konfigurasi AudioMixer."
            );
        }
    }
}