using RoR2.ContentManagement;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using RoR2EditorKit.Core.Windows;
using RoR2EditorKit.RoR2.EditorWindows;
using RoR2EditorKit.Core.Inspectors;

namespace RoR2EditorKit.RoR2.Inspectors
{
    [CustomEditor(typeof(SerializableContentPack))]
    public class SerializableContentPackCustomEditor : ExtendedInspector
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceID, int line)
        {
            if(enableInspectors)
            {
                SerializableContentPack obj = EditorUtility.InstanceIDToObject(instanceID) as SerializableContentPack;
                if (obj != null)
                {
                    ExtendedEditorWindow.OpenEditorWindow<SerializableContentPackEditorWindow>(obj, "Serializable Content Pack Window");
                    return true;
                }
            }
            return false;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (enableInspectors && GUILayout.Button("Open Editor"))
            {
                ExtendedEditorWindow.OpenEditorWindow<SerializableContentPackEditorWindow>(target, "Serializable Content Pack Window");
            }
        }
    }
}