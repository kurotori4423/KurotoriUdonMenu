#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UdonSharpEditor;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.Events;

[System.Serializable]
public class LanguageOption : IOptionItem
{
    [SerializeField] UdonBehaviour languageSwitcher;

    public void CreateOption(OptionsSettings settings)
    {
        var assetPath = "Assets/KurotoriUdonMenu/KurotoriUdonMenu2/Scripts/Options/Prefabs/LanguageOption.prefab";

        var prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

        var gameObject = settings.InstantiateObject(prefab, settings.menuParent);
        gameObject.name = "LanguageOption";

        var toggles = gameObject.GetComponentsInChildren<Toggle>();
        var jpToggleObj = toggles.Where(e => e.gameObject.name.Equals("Japanese")).First();
        var enToggleObj = toggles.Where(e => e.gameObject.name.Equals("English")).First();

        var switcher = languageSwitcher.GetUdonSharpComponent<Kurotori.UdonMenuLanguageSwitcher>();

        switcher.UpdateProxy();
        switcher.enToggle = enToggleObj;
        switcher.jpToggle = jpToggleObj;
        switcher.ApplyProxyModifications();

        EditorUtility.SetDirty(languageSwitcher);

        // toggleにUdonをセットする
        using (var toggleSo = new SerializedObject(jpToggleObj))
        {
            using (var callProperty = toggleSo.FindProperty("onValueChanged.m_PersistentCalls.m_Calls"))
            {
                callProperty.arraySize = 0;
                callProperty.arraySize = 1;
                using (var element = callProperty.GetArrayElementAtIndex(0))
                {
                    element.FindPropertyRelative("m_Target").objectReferenceValue = languageSwitcher;
                    element.FindPropertyRelative("m_MethodName").stringValue = "SendCustomEvent";
                    element.FindPropertyRelative("m_Mode").enumValueIndex = (int)PersistentListenerMode.String;
                    element.FindPropertyRelative("m_Arguments.m_StringArgument").stringValue = "ChangeJP";
                    element.FindPropertyRelative("m_CallState").enumValueIndex = (int)UnityEventCallState.RuntimeOnly;
                }
            }

            toggleSo.ApplyModifiedProperties();
        }

        using (var toggleSo = new SerializedObject(enToggleObj))
        {
            using (var callProperty = toggleSo.FindProperty("onValueChanged.m_PersistentCalls.m_Calls"))
            {
                callProperty.arraySize = 0;
                callProperty.arraySize = 1;
                using (var element = callProperty.GetArrayElementAtIndex(0))
                {
                    element.FindPropertyRelative("m_Target").objectReferenceValue = languageSwitcher;
                    element.FindPropertyRelative("m_MethodName").stringValue = "SendCustomEvent";
                    element.FindPropertyRelative("m_Mode").enumValueIndex = (int)PersistentListenerMode.String;
                    element.FindPropertyRelative("m_Arguments.m_StringArgument").stringValue = "ChangeEN";
                    element.FindPropertyRelative("m_CallState").enumValueIndex = (int)UnityEventCallState.RuntimeOnly;
                }
            }
            toggleSo.ApplyModifiedProperties();
        }
    }
}
#endif