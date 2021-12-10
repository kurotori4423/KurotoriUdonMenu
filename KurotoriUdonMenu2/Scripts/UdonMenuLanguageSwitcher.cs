
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Kurotori
{
#if !COMPILER_UDONSHARP && UNITY_EDITOR

    using UnityEditor;
    using System.Linq;
    using UdonSharpEditor;

    [CustomEditor(typeof(UdonMenuLanguageSwitcher))]
    public class UdonMenuLanguageSwitcherInspector : Editor
    {
        SerializedProperty _root;
        SerializedProperty _jpLabels;
        SerializedProperty _enLabels;
        SerializedProperty _jpToggle;
        SerializedProperty _enToggle;

        private bool _jpLabelsOpen = false;
        private bool _enLabelsOpen = false;

        private void OnEnable()
        {
            var udonMenuLanguageSwitcher = target as UdonMenuLanguageSwitcher;

            var so = new SerializedObject(udonMenuLanguageSwitcher);

            _root = so.FindProperty("root");
            _jpLabels = so.FindProperty("jpLabels");
            _enLabels = so.FindProperty("enLabels");
            _jpToggle = so.FindProperty("jpToggle");
            _enToggle = so.FindProperty("enToggle");
        }

        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target))
                return;
            
            EditorGUILayout.PropertyField(_root);
            EditorGUILayout.PropertyField(_jpToggle);
            EditorGUILayout.PropertyField(_enToggle);

            if (GUILayout.Button("Collect Labels"))
            {

                var labels = (_root.objectReferenceValue as Transform).gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);

                var jpLabels = labels.Where(e => e.gameObject.name.Contains("_JP")).Select(v => v.gameObject).ToList();
                var enLabels = labels.Where(e => e.gameObject.name.Contains("_EN")).Select(v => v.gameObject).ToList();

                var jpLabelCount = jpLabels.Count();
                _jpLabels.arraySize = 0;
                _jpLabels.arraySize = jpLabelCount;
                for (int i = 0; i < jpLabelCount; ++i)
                {
                    using (var element = _jpLabels.GetArrayElementAtIndex(i))
                    {
                        element.objectReferenceValue = jpLabels[i];
                    }
                }

                var enLabelCount = enLabels.Count();
                _enLabels.arraySize = 0;
                _enLabels.arraySize = enLabelCount;
                for (int i = 0; i < enLabelCount; ++i)
                {
                    using (var element = _enLabels.GetArrayElementAtIndex(i))
                    {
                        element.objectReferenceValue = enLabels[i];
                    }
                }

                _enLabels.serializedObject.ApplyModifiedProperties();
            }

            using (new EditorGUI.DisabledScope(true))
            {
                _jpLabelsOpen = EditorGUILayout.Foldout(_jpLabelsOpen, "JP Label : " + _jpLabels.arraySize);

                if(_jpLabelsOpen)
                {
                    for(var i = 0; i < _jpLabels.arraySize; ++i)
                    {
                        EditorGUILayout.ObjectField(_jpLabels.GetArrayElementAtIndex(i).objectReferenceValue, typeof(GameObject), true);
                    }
                }

                _enLabelsOpen = EditorGUILayout.Foldout(_enLabelsOpen, "EN Label : " + _enLabels.arraySize);

                if (_enLabelsOpen)
                {
                    for (var i = 0; i < _enLabels.arraySize; ++i)
                    {
                        EditorGUILayout.ObjectField(_enLabels.GetArrayElementAtIndex(i).objectReferenceValue, typeof(GameObject), true);
                    }
                }
            }

            _root.serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    public class UdonMenuLanguageSwitcher : UdonSharpBehaviour
    {
        [SerializeField] public Transform root;
        [SerializeField] public GameObject[] jpLabels;
        [SerializeField] public GameObject[] enLabels;
        [SerializeField] bool modeJP = true;
        [SerializeField] public Toggle jpToggle;
        [SerializeField] public Toggle enToggle;

        void Start()
        {
            SetLanguage();
        }

        void SetLanguage()
        {
            foreach (var jpLabel in jpLabels)
            {
                jpLabel.SetActive(modeJP);
            }

            foreach (var enLabel in enLabels)
            {
                enLabel.SetActive(!modeJP);
            }
        }

        public void ChangeJP()
        {
            Debug.Log("Change_JP");
            modeJP = true;
            SetLanguage();
        }

        public void ChangeEN()
        {
            Debug.Log("Change_EN");
            modeJP = false;
            SetLanguage();
        }

        public void ChangeJPOther()
        {
            jpToggle.isOn = true;
            enToggle.isOn = false;
            modeJP = true;
        }

        public void ChangeENOther()
        {
            jpToggle.isOn = false;
            enToggle.isOn = true;
            modeJP = false;
        }
    }
}
