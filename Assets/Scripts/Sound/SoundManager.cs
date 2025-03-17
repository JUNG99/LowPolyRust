using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerSound
{
    Walk,
    Pick,    // 아이템 줏을때

}

public enum BGM
{
    Lobby
}

public enum UISound
{
    Click
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance { get => instance; }

    private void Awake()
    {
        // 이미 인스턴스가 존재하고 현재 오브젝트가 아니라면
        if (instance != null && instance != this)
        {
            // 중복된 인스턴스 제거
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없으면 현재 인스턴스 설정
        instance = this;
    }

    [Header("===Player Sound===")]
    [SerializeField] AudioClip[] playerClip;

    [Header("===Background Sound===")]
    [SerializeField] AudioClip[] backGroundClip;

    [Header("===Dungeon Sound===")]
    [SerializeField] AudioClip[] dungeonSound;

    [Header("===Ui Sound===")]
    [SerializeField] AudioClip[] uiClickSound;

    /// <summary>
    /// 사용방법
    /// 1. 프리팹에 AudioSource컴포넌트를 할당한다
    /// 2. AudioSource 변수를 만든 후, Awake/Start에서 Audio GetComponent 
    /// 3. SoundManager.Instance.PlayPlayerSound(오디오소스, enum);
    /// 
    /// play : source에 미리 clip이 할당되어있어야 함, 재생중인 소리 중지 시킨 후 재생
    /// PlayOnShot : 재생시킬 clip 매개변수로, 재생중지 하지 않고 같이 재생
    /// </summary>

    // 플레이어 사운드
    public void PlaySounds(AudioSource audioSource, PlayerSound sound)
    {
        try
        {
            audioSource.PlayOneShot(playerClip[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

    // 배경 사운드
    public void PlaySounds(AudioSource audioSource, BGM sound)
    {
        try
        {
            //audioSource.PlayOneShot(backGroundClip[(int)sound]);

            // 현재 재생 중인 사운드가 있으면 중지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // 새 클립 설정 후 재생
            audioSource.clip = backGroundClip[(int)sound];
            audioSource.Play();
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

    // Ui 사운드
    public void PlaySounds(AudioSource audioSource, UISound sound)
    {
        try
        {
            audioSource.PlayOneShot(uiClickSound[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

    // 사운드가 본인 Manager에서 사용하고 있다면
    public void PlaySounds(AudioSource audioSource, AudioClip sound)
    {
        try
        {
            audioSource.PlayOneShot(sound);
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

}
