using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;
using UdonSharpEditor;
using System.Linq;

namespace Kurotori.UdonMenu
{

    public interface IOptionItem
    {
        GameObject CreateOption(OptionsSettings settings);

    }

    [System.Serializable]
    public class GameObjectToggleOption : IOptionItem
    {
        public GameObject target;
        [SerializeField] bool defaultOn;
        [SerializeField] string jpLabel;
        [SerializeField] string enLabel;
        public GameObject CreateOption(OptionsSettings settings)
        {
            Debug.Log("GameObjectOption");

            var assetPath = "Assets/KurotoriUdonMenu/KurotoriUdonMenu2/Scripts/Options/Prefabs/GameObjectToggle.prefab";

            var prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            if (prefab == null) Debug.Log("null");

            var gameObject = settings.InstantiateObject(prefab, settings.menuParent);
            gameObject.name = "ToggleGameObjectOption_" + target.name;

            var labelJP = gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>().Where(e => e.gameObject.name.Equals("Label_JP")).First();

            using (var so = new SerializedObject(labelJP))
            {
                var sp = so.FindProperty("m_text");
                sp.stringValue = jpLabel;
                so.ApplyModifiedProperties();
            }

            var labelEN = gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>().Where(e => e.gameObject.name.Equals("Label_EN")).First();

            using (var so = new SerializedObject(labelEN))
            {
                var sp = so.FindProperty("m_text");
                sp.stringValue = enLabel;
                so.ApplyModifiedProperties();
            }
            var udon = gameObject.GetUdonSharpComponent<UdonMenuGameObjectSwitch>();

            udon.UpdateProxy();
            udon.switchObject = target;
            udon.isOn = defaultOn;
            udon.ApplyProxyModifications();

            return gameObject;
        }
    }

    [System.Serializable]
    public class AudioVolumeSliderOption : IOptionItem
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] float initVolume;
        [SerializeField] string jpLabel;
        [SerializeField] string enLabel;
        public GameObject CreateOption(OptionsSettings settings)
        {
            var assetPath = "Assets/KurotoriUdonMenu/KurotoriUdonMenu2/Scripts/Options/Prefabs/AudioVolumeSlider.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            var gameObject = settings.InstantiateObject(prefab, settings.menuParent);
            gameObject.name = "AudioVolumeSlider_" + audioSource.gameObject.name;

            var labelJP = gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>().Where(e => e.gameObject.name.Equals("Label_JP")).First();

            using (var so = new SerializedObject(labelJP))
            {
                var sp = so.FindProperty("m_text");
                sp.stringValue = jpLabel;
                so.ApplyModifiedProperties();
            }

            var labelEN = gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>().Where(e => e.gameObject.name.Equals("Label_EN")).First();

            using (var so = new SerializedObject(labelEN))
            {
                var sp = so.FindProperty("m_text");
                sp.stringValue = enLabel;
                so.ApplyModifiedProperties();
            }

            var udon = gameObject.GetUdonSharpComponent<UdonMenuAudioVolumeSlider>();

            udon.UpdateProxy();
            udon.audioSource = audioSource;
            udon.initVolume = initVolume;
            udon.ApplyProxyModifications();

            return gameObject;
        }
    }

    [System.Serializable]
    public class AnimatorSliderOption : IOptionItem
    {
        [SerializeField] Animator animator;
        [SerializeField] string jpLabel;
        [SerializeField] string enLabel;
        public GameObject CreateOption(OptionsSettings settings)
        {
            var assetPath = "Assets/KurotoriUdonMenu/KurotoriUdonMenu2/Scripts/Options/Prefabs/AnimatorSlider.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            var gameObject = settings.InstantiateObject(prefab, settings.menuParent);
            gameObject.name = "AnimatorSlider";

            var labelJP = gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>().Where(e => e.gameObject.name.Equals("Label_JP")).First();

            using (var so = new SerializedObject(labelJP))
            {
                var sp = so.FindProperty("m_text");
                sp.stringValue = jpLabel;
                so.ApplyModifiedProperties();
            }

            var labelEN = gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>().Where(e => e.gameObject.name.Equals("Label_EN")).First();

            using (var so = new SerializedObject(labelEN))
            {
                var sp = so.FindProperty("m_text");
                sp.stringValue = enLabel;
                so.ApplyModifiedProperties();
            }

            var udon = gameObject.GetUdonSharpComponent<UdonMenuAnimatorSlider>();

            udon.UpdateProxy();
            udon.animator = animator;
            udon.initValue = 0.5f;
            udon.ApplyProxyModifications();

            return gameObject;
        }
    }

    [CustomEditor(typeof(OptionsSettings))]
    public class OptionsSettingsEditor : Editor
    {
        private ReorderableList _reorderableList; // ReorderableListを利用して、並び替えや+-ボタンを使えるようにする

        SerializedProperty m_item;

        private void OnEnable()
        {
            _reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("items"));

            _reorderableList.drawElementCallback += (Rect rect, int index, bool selected, bool focused) =>
            {
                SerializedProperty property = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            // PropertyFieldを使ってよしなにプロパティの描画を行う（PropertyDrawerを使っているのでそちらに移譲されます）
            EditorGUI.PropertyField(rect, property);
            };

            _reorderableList.drawHeaderCallback += rect =>
            {
                EditorGUI.LabelField(rect, "Options");
            };

            _reorderableList.elementHeightCallback = (index) =>
            {
                return EditorGUI.GetPropertyHeight(_reorderableList.serializedProperty.GetArrayElementAtIndex(index)) + EditorGUIUtility.standardVerticalSpacing;
            };
        }

        public override void OnInspectorGUI()
        {
            OptionsSettings settings = target as OptionsSettings;

            serializedObject.Update();

            EditorGUILayout.LabelField("OptionParent");
            settings.menuParent = EditorGUILayout.ObjectField("Option Parent", settings.menuParent, typeof(Transform), true) as Transform;

            _reorderableList.DoLayoutList();

            //EditorGUILayout.PropertyField(serializedObject.FindProperty("items"), true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Generate Menu"))
            {
                var childCount = settings.menuParent.childCount;

                // 指定の階層以下を削除する
                GameObject[] children = new GameObject[settings.menuParent.childCount];
                children = settings.menuParent.Cast<Transform>().Select(e => e.gameObject).ToArray();

                foreach (var child in children)
                {
                    DestroyImmediate(child);
                }

                var optionObjectList = new List<GameObject>();

                // メニュー項目を生成する。
                foreach (var item in settings.items)
                {
                    if (item != null)
                    {
                        var go = item.CreateOption(settings);
                        optionObjectList.Add(go);
                    }
                }

                var optionApplyer = settings.gameObject.GetUdonSharpComponent<UdonMenuOptionApplyer>();

                optionApplyer.UpdateProxy();
                optionApplyer.options = optionObjectList.ToArray();
                optionApplyer.ApplyProxyModifications();

                var optionApplyerUdon = UdonSharpEditorUtility.GetBackingUdonBehaviour(optionApplyer);

                EditorUtility.SetDirty(optionApplyerUdon);
            }
        }


    }

    public class OptionsSettings : MonoBehaviour
    {

        [SerializeReference, SubclassSelector]
        public List<IOptionItem> items;

        public Transform menuParent;

        public GameObject InstantiateObject(GameObject gameObject, Transform parent)
        {
            var obj = PrefabUtility.InstantiatePrefab(gameObject) as GameObject;

            var localPos = obj.transform.localPosition;
            var localRot = obj.transform.localRotation;
            var localScale = obj.transform.localScale;

            obj.transform.parent = parent;

            obj.transform.localPosition = localPos;
            obj.transform.localRotation = localRot;
            obj.transform.localScale = localScale;

            return obj;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
#endif