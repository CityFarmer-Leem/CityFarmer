using System;
using UnityEngine;

public enum SoundType
{
    BGM,
    SFX,
    Voice,
    NumOfSoundType
}

public class SoundManager : MonoBehaviour
{
    private AudioSource[] _audioSources = new AudioSource[(int)SoundType.NumOfSoundType];

    public void Init()
    {
        for (int index = 0; index < _audioSources.Length; ++index)
        {
            GameObject gameObject = new GameObject(Enum.GetName(typeof(SoundType), index));
            gameObject.AddComponent<AudioSource>();
            gameObject.transform.parent = transform;

            _audioSources[index] = transform.GetChild(index).GetComponent<AudioSource>();

        }
        _audioSources[(int)SoundType.BGM].loop = true;
    }

    public void Clear(AudioSource[] audioSources)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }

    public void Play(SoundType sound, string fileName)
    {
        // TODO : �Ҹ� ������ ���� �÷��� ����

        if (sound == SoundType.Voice)
        {

        }

    }

    public AudioClip GetAudioClip()
    {
        return null;
    }

    public void SetVolume(SoundType soundType, float value)
    {
        //���� ���� ����ϴ� ���� ���� ��. ������ ����� 50�� �ƴ� 20�� ����ϳ�, ��ȭ�� �ѷ����� �ʾ� 50�� �����.
        _audioSources[(int)soundType].volume = value;
    }
}
