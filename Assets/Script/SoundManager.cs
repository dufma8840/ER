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
    // 게임 시작 시 호출되는 함수
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
        // Dictionary 초기화
        audioClipsDic = new Dictionary<string, AudioClip>();

        // 사운드 리소스를 Dictionary에 추가
        foreach (AudioClip clip in soundClips)
        {
            audioClipsDic.Add(clip.name, clip);
        }
    }

    // 특정 이름에 해당하는 사운드 클립을 반환하는 함수
    public AudioClip GetSoundClip(string soundName)
    {
        AudioClip clip;
        if (audioClipsDic.TryGetValue(soundName, out clip))
        {            
            return clip;
        }
        else
        {
            Debug.LogWarning("사운드 이름이 올바르지 않습니다: " + soundName);
            return null;
        }
    }

    public void BearSound()
    {
        bearSound.clip = GetSoundClip("곰");
        bearSound.Play();
    }
    public void MinoSound()
    {
        minoSound.clip = GetSoundClip("미노");
        minoSound.Play();

    }

}
