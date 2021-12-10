
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class LanguageChanger : UdonSharpBehaviour
{
    [SerializeField]
    GameObject[] JapaneseObjects = null;

    [SerializeField]
    GameObject[] EnglishObjects = null;

    [SerializeField]
    UdonMenu udonMenu = null;

    [SerializeField]
    Toggle japaneseToggle = null;

    [SerializeField]
    Toggle englishToggle = null;

    void Start()
    {
        
    }

    public void EnglishOnOther()
    {
        englishToggle.isOn = true;
        japaneseToggle.isOn = false;
    }

    public void JapaneseOnOther()
    {
        japaneseToggle.isOn = true;
        englishToggle.isOn = false;

    }

    public void EnglishOn()
    {
        if (englishToggle.isOn)
        {
            foreach (var japanese in JapaneseObjects)
            {
                japanese.SetActive(false);
            }

            foreach (var english in EnglishObjects)
            {
                english.SetActive(true);
            }

            udonMenu.SetEnglish();
        }
    }

    public void JapaneseOn()
    {
        if (japaneseToggle.isOn)
        {
            foreach (var japanese in JapaneseObjects)
            {
                japanese.SetActive(true);
            }

            foreach (var english in EnglishObjects)
            {
                english.SetActive(false);
            }

            udonMenu.SetJapanese();
        }
    }
}
