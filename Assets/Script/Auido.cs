using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Auido : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    public void SetLevel(float sliderVal)
    {
        sliderVal = slider.value;
        mixer.SetFloat("GameSound", Mathf.Log10(sliderVal) * 20);
    }

}