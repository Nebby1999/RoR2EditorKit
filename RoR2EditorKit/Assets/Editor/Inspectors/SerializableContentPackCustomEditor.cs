using RoR2.ContentManagement;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RoR2EditorKit
{
    [CustomEditor(typeof(SerializableContentPack))]
    public class SerializableContentPackCustomEditor : Editor
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceID, int line)
        {
            SerializableContentPack obj = EditorUtility.InstanceIDToObject(instanceID) as SerializableContentPack;
            if (obj != null)
            {
                ExtendedEditorWindow.OpenEditorWindow<SerializableContentPackEditorWindow>(obj, "Serializable Content Pack Window");
                return true;
            }
            return false;
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                ExtendedEditorWindow.OpenEditorWindow<SerializableContentPackEditorWindow>(target, "Serializable Content Pack Window");
            }
        }
    }
}