using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using BalDUtilities.Misc;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfx2DSource;
    [SerializeField] private AudioSource[] soundsSource;

    [SerializeField] private AudioMixer mainMixer;

    public const string masterVolParam = "MasterVol";
    public const string musicVolParam = "MusicVol";
    public const string sfxVolparam = "SFXVol";

    [Header("Sliders")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Space]
    [SerializeField] private float volMultiplier = 30f;

    [System.Serializable]
    public enum E_ClipsTags
    {
        M_MainMenu,
        M_MainScene,

        S_Clic,
        S_DialogueBase,
    }

    [System.Serializable]
    public struct ClipByTag
    {
#if UNITY_EDITOR
        public string inEditorName;
#endif
        public E_ClipsTags tag;
        public AudioClip clip;
    }

    [SerializeField] private ClipByTag[] musicClipsByTag;
    [SerializeField] private ClipByTag[] sfxClipsByTag;

    private void Start()
    {
        SetupSliders();
        LoadSlidersValue();
    }

    private void SetupSliders()
    {
        if (mainSlider == null) mainSlider = SearchSlider("main");
        if (musicSlider == null) musicSlider = SearchSlider("music");
        if (sfxSlider == null) sfxSlider = SearchSlider("sfx");

        mainSlider?.onValueChanged.AddListener(OnMainSliderValueChange);
        musicSlider?.onValueChanged.AddListener(OnMusicSliderValueChange);
        sfxSlider?.onValueChanged.AddListener(OnSFXSliderValueChange);
    }

    private Slider SearchSlider(string searchedName)
    {
        Debug.LogError(searchedName + " slider was not set in AudioManager.", this.gameObject);

        Slider[] objs = GameObject.FindObjectsOfType<Slider>();
        foreach (var item in objs)
        {
            if (item.name.Contains(searchedName, System.StringComparison.InvariantCultureIgnoreCase)) return item;
        }

        return null;
    }

    /// <summary>
    /// Loads the sliders value from <seealso cref="SaveManager.GetSavedFloatKey(SaveManager.E_SaveKeys)"/>
    /// </summary>
    private void LoadSlidersValue()
    {
        mainSlider.value = SaveManager.GetSavedFloatKey(SaveManager.E_SaveKeys.F_MasterVolume);
        musicSlider.value = SaveManager.GetSavedFloatKey(SaveManager.E_SaveKeys.F_MusicVolume);
        sfxSlider.value = SaveManager.GetSavedFloatKey(SaveManager.E_SaveKeys.F_SFXVolume);
    }

    public void OnMainSliderValueChange(float value) => HandleSliderChange(value, masterVolParam, SaveManager.E_SaveKeys.F_MasterVolume);
    public void OnMusicSliderValueChange(float value) => HandleSliderChange(value, musicVolParam, SaveManager.E_SaveKeys.F_MusicVolume);
    public void OnSFXSliderValueChange(float value) => HandleSliderChange(value, sfxVolparam, SaveManager.E_SaveKeys.F_SFXVolume);

    /// <summary>
    /// Changes the <paramref name="param"/> key in the <seealso cref="mainMixer"/> with <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="param"></param>
    /// <param name="key"></param>
    private void HandleSliderChange(float value, string param, SaveManager.E_SaveKeys key)
    {
        float newVol = 0;
        if (value > 0) newVol = Mathf.Log10(value) * volMultiplier;
        else newVol = -80f;

        mainMixer.SetFloat(param, newVol);
        SaveManager.SetSavedKey(key, value);

    }

    /// <summary>
    /// Plays the sound with <paramref name="key"/> as tag, stored in <seealso cref="sfxClipsByTag"/>.
    /// </summary>
    /// <param name="key"></param>
    public void Play2DSFX(E_ClipsTags key)
    {
        foreach (var item in sfxClipsByTag)
        {
            if (item.tag.Equals(key))
            {
                sfx2DSource.PlayOneShot(item.clip);
                return;
            }
        }

        Debug.LogError("Could not find " + EnumsExtension.EnumToString(key) + " in sfxClipsByTag");
    }

    /// <summary>
    /// Plays the sound with <paramref name="key"/> as tag, stored in <seealso cref="sfxClipsByTag"/>.
    /// </summary>
    /// <param name="key"></param>
    public void Play2DSFX(string key)
    {
        foreach (var item in sfxClipsByTag)
        {
            if (EnumsExtension.EnumToString(item.tag).Equals(key))
            {
                sfx2DSource.PlayOneShot(item.clip);
                return;
            }
        }

        Debug.LogError("Could not find " + key + " in sfxClipsByTag");
    }

    public AudioClip GetClip(E_ClipsTags tag)
    {
        foreach (var item in sfxClipsByTag)
        {
            if (item.tag == tag) return item.clip;
        }

        return null;
    }

    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();
    public void StopMusic() => musicSource.Stop();
    public void PlayMusic(E_ClipsTags musicTag)
    {
        foreach (var item in musicClipsByTag)
        {
            if (item.tag.Equals(musicTag))
            {
                musicSource.clip = item.clip;
                musicSource.Play();
            }
        }
    }
}
