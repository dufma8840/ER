using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] soundClips;
    private Dictionary<string, AudioClip> audioClipsDic;
    public AudioSource bearSound;
    public AudioSource minoSound;
    static SoundManager Instance;
    // ���� ���� �� ȣ��Ǵ� �Լ�
    public static SoundManager GetInstance()
    {
        return Instance;
    }
    private void Awake()
    {
        Instance = this;
        bearSound = GetComponents<AudioSource>()[0];
        minoSound = GetComponents<AudioSource>()[1];
    }
    private void Start()
    {
        // Dictionary �ʱ�ȭ
        audioClipsDic = new Dictionary<string, AudioClip>();

        // ���� ���ҽ��� Dictionary�� �߰�
        foreach (AudioClip clip in soundClips)
        {
            audioClipsDic.Add(clip.name, clip);
        }
    }

    // Ư�� �̸��� �ش��ϴ� ���� Ŭ���� ��ȯ�ϴ� �Լ�
    public AudioClip GetSoundClip(string soundName)
    {
        AudioClip clip;
        if (audioClipsDic.TryGetValue(soundName, out clip))
        {            
            return clip;
        }
        else
        {
            Debug.LogWarning("���� �̸��� �ùٸ��� �ʽ��ϴ�: " + soundName);
            return null;
        }
    }

    public void BearSound()
    {
        bearSound.clip = GetSoundClip("��");
        bearSound.Play();
    }
    public void MinoSound()
    {
        minoSound.clip = GetSoundClip("�̳�");
        minoSound.Play();

    }

}
