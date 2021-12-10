#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UdonSharpEditor;

[System.Serializable]
public class LanguageOption : IOptionItem
{


    public void CreateOption(OptionsSettings settings)
    {
        var assetPath = "Assets/KurotoriUdonMenu/KurotoriUdonMenu2/Scripts/Options/Prefabs/LanguageOption.prefab";

        var prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

        var gameObject = settings.InstantiateObject(prefab, settings.menuParent);
        gameObject.name = "LanguageOption";

        //var udon = gameObject.GetUdonSharpComponent<UdonMenuPlayerVoiceRangeSlider>();

        //udon.UpdateProxy();

        //udon.minVoiceRange = minVocieRange;
        //udon.maxVoiceRange = maxVoiceRange;

        //udon.ApplyProxyModifications();
    }
}

#endif