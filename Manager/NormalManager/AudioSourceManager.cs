using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//负责控制音乐的播放和停止以及游戏中各种音效的播放
public class AudioSourceManager 
{
    private AudioSource [] audioSource;
    private bool playEffectMusic=true;
    private bool playBGMusic=true;

    public AudioSourceManager()
    {
        audioSource = GameManager.Instance.GetComponents<AudioSource>();
        audioSource[0].loop = true;//设置为循环播放
    }
    public void PlayBGMusic(AudioClip audioClip)//切换背景音乐
    {
        if (audioSource[0].clip!=audioClip)
        {
            audioSource[0].clip = audioClip;
            if (playBGMusic)
            {
                audioSource[0].Play();
            }
        }
    }

    public void PlayEffectMusic(AudioClip audioClip)//播放给定的音乐一次，例如怪物死亡音效
    {
        if (playEffectMusic)
        {
            audioSource[1].PlayOneShot(audioClip);
            
        }
    }

    public void CloseBGMusic()//关闭背景音乐
    {
        audioSource[0].Stop();
    }

    public void OpenBGMusic()//打开背景音乐
    {
        audioSource[0].Play();
    }

    public void CloseEffectMusic()//关闭游戏音乐
    {
        audioSource[1].Stop();
    }

    public void OpenEffectMusic()//打开游戏音乐
    {
        audioSource[1].Play();
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        if (playBGMusic == false)
        {
            CloseBGMusic();
        }
        else
        {
            OpenBGMusic();
        }
    }
    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
        if (playEffectMusic == false)
        {
            CloseEffectMusic();
        }
        else
        {
            OpenEffectMusic();
        }
    }
    //按钮音效播放
    public void PlayButton()
    {
        PlayEffectMusic(GameManager.Instance.GetAudioClip("AudioClips/Main/Button"));
    }
    //翻书音效播放
    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.GetAudioClip("AudioClips/Main/Paging"));
    }
}