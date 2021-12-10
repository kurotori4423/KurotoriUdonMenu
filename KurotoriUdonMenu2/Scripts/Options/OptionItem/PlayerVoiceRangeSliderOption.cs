using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using UdonSharpEditor;

[System.Serializable]
public class PlayerVoiceRangeSliderOption : IOptionItem
{
    public float minVocieRange;
    public float maxVoiceRange;
    public float initRange;

    public void CreateOption(OptionsSettings settings)
    {
        var assetPath = "Assets/KurotoriUdonMenu/KurotoriUdonMenu2/Scripts/Options/Prefabs/PlayerVoiceRangeSlider.prefab";

        var prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

        var gameObject = settings.InstantiateObject(prefab, settings.menuParent);
        gameObject.name = "PlayerVoiceRangeSlider";

        var udon = gameObject.GetUdonSharpComponent<UdonMenuPlayerVoiceRangeSlider>();

        udon.UpdateProxy();

        udon.minVoiceRange = minVocieRange;
        udon.maxVoiceRange = maxVoiceRange;
        udon.initRange = initRange;

        udon.ApplyProxyModifications();
    }
}

#endif