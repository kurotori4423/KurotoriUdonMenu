
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UdonMenuAudioVolumeSlider : UdonSharpBehaviour
{
    public AudioSource audioSource;
    public float initVolume;
    [SerializeField] Slider slider;
    
    float CalcVolume(float sliderValue)
    {
        // USharpVideoと同じ音量調整
        // https://www.dr-lex.be/info-stuff/volumecontrols.html#ideal thanks TCL for help with finding and understanding this
        // Using the 50dB dynamic range constants
        return Mathf.Clamp01(3.1623e-3f * Mathf.Exp(sliderValue * 5.757f) - 3.1623e-3f);
    }


    void Start()
    {
        slider.SetValueWithoutNotify(initVolume);
        audioSource.volume = CalcVolume(initVolume);
    }

    public void OnValueChange()
    {
        audioSource.volume = CalcVolume(slider.value);
    }
}
