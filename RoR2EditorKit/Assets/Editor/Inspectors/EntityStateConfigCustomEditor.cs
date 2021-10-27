using RoR2;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RoR2EditorKit
{
    [CustomEditor(typeof(EntityStateConfiguration))]
    public class EntityStateConfigCustomEditor : Editor
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceID, int line)
        {
            EntityStateConfiguration obj = EditorUtility.InstanceIDToObject(instanceID) as EntityStateConfiguration;
            if (obj != null)
            {
                ExtendedEditorWindow.OpenEditorWindow<EntityStateConfigEditorWindow>(obj, "Entity State Configuration Editor");
                return true;
            }
            return false;
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                ExtendedEditorWindow.OpenEditorWindow<EntityStateConfigEditorWindow>(target, "Entity State Configuration Editor");
            }
        }
    }
}
