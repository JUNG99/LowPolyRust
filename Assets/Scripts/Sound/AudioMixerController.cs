using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider materVolumeSlider;
    [SerializeField]
    private Slider bgmVolumeSlider;
    [SerializeField]
    private Slider sfxSlider;

    private void Awake()
    {
        materVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }

    private void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(value) * 20);
    }

    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }
}
