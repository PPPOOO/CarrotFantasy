using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager
{
    private AudioSource[] audioSources;//0.播放BGM;1.播放特效
    private bool playEffectMusic = true;
    private bool playBGMusic = true;

    public AudioSourceManager()
    {
        audioSources = GameManager.Instance.GetComponents<AudioSource>();
    }

    public void PlayBGMusic(AudioClip audioClip)
    {
        if (!audioSources[0].isPlaying||audioSources[0].clip!=audioClip)
        {
            audioSources[0].clip = audioClip;
            audioSources[0].Play();
        }
    }

    public void PlayEffectMusic(AudioClip audioClip)
    {
        if (playEffectMusic)
        {
            audioSources[1].PlayOneShot(audioClip);
        }
    }

    public void CloseBGMusic()
    {
        audioSources[0].Stop();
    }

    public void OpenBGMusic()
    {
        audioSources[0].Play();
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        if (playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }
    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
    }

    public void PlayButtonAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Button"));
    }

    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Paging"));
    }
}
