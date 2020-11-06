using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using VRC.Udon.Editor;
using System.Text;

namespace HDAssets.UdonExtAsm
{
    [ScriptedImporter(1, "uextasm")]
    [UsedImplicitly]
public class UdonExtAsmImporter : ScriptedImporter
{
        public override void OnImportAsset(AssetImportContext ctx)
        {
            UdonExtAsm udonExtAsm = ScriptableObject.CreateInstance<UdonExtAsm>();
            SerializedObject serializedUdonExtAsm = new SerializedObject(udonExtAsm);
            SerializedProperty udonExtAsmProperty = serializedUdonExtAsm.FindProperty("udonAssembly");
            var fs = new FileStream(ctx.assetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            udonExtAsmProperty.stringValue = new StreamReader( fs, Encoding.UTF8 ).ReadToEnd();
            serializedUdonExtAsm.ApplyModifiedProperties();

            udonExtAsm.RefreshProgram();
            udonExtAsm.compiled = false;

            ctx.AddObjectToAsset("Imported External Udon Assembly Program", udonExtAsm);
            ctx.SetMainObject(udonExtAsm);
        }
    }
}