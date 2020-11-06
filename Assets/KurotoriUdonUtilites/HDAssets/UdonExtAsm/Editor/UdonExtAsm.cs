using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Editor;
using VRC.Udon.Editor.ProgramSources;
using VRC.Udon.Editor.ProgramSources.Attributes;
using System.IO;

//[assembly: UdonProgramSourceNewMenu(typeof(HDAssets.UdonExtAsm.UdonExtAsm), "Udon External Assembly Program Asset")]

namespace HDAssets.UdonExtAsm
{
    public class UdonExtAsm : UdonAssemblyProgramAsset
    {
        [SerializeField]
        [HideInInspector]
        public bool compiled = false;
        protected override void DrawProgramSourceGUI(UdonBehaviour udonBehaviour, ref bool dirty)
        {
            DrawBuildButton();
            DrawPublicVariables(udonBehaviour, ref dirty);
            DrawAssemblyErrorTextArea();
        }
        [PublicAPI]
        protected void DrawBuildButton()
        {
            EditorGUILayout.LabelField("Compile", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(compiled ? "COMPILED" : "NOT COMPILE", EditorStyles.label);
            if (GUILayout.Button("Compile Program"))
            {
                UdonEditorManager.Instance.QueueAndRefreshProgram(this);
                compiled = true;
            }
        }
    }
}
