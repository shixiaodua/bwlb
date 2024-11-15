using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������ֵĲ��ź�ֹͣ�Լ���Ϸ�и�����Ч�Ĳ���
public class AudioSourceManager 
{
    private AudioSource [] audioSource;
    private bool playEffectMusic=true;
    private bool playBGMusic=true;

    public AudioSourceManager()
    {
        audioSource = GameManager.Instance.GetComponents<AudioSource>();
        audioSource[0].loop = true;//����Ϊѭ������
    }
    public void PlayBGMusic(AudioClip audioClip)//�л���������
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

    public void PlayEffectMusic(AudioClip audioClip)//���Ÿ���������һ�Σ��������������Ч
    {
        if (playEffectMusic)
        {
            audioSource[1].PlayOneShot(audioClip);
            
        }
    }

    public void CloseBGMusic()//�رձ�������
    {
        audioSource[0].Stop();
    }

    public void OpenBGMusic()//�򿪱�������
    {
        audioSource[0].Play();
    }

    public void CloseEffectMusic()//�ر���Ϸ����
    {
        audioSource[1].Stop();
    }

    public void OpenEffectMusic()//����Ϸ����
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
    //��ť��Ч����
    public void PlayButton()
    {
        PlayEffectMusic(GameManager.Instance.GetAudioClip("AudioClips/Main/Button"));
    }
    //������Ч����
    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.GetAudioClip("AudioClips/Main/Paging"));
    }
}