using RoR2;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using RoR2EditorKit.Core.Windows;
using RoR2EditorKit.RoR2.EditorWindows;
using RoR2EditorKit.Core.Inspectors;

namespace RoR2EditorKit.RoR2.Inspectors
{
    [CustomEditor(typeof(EntityStateConfiguration))]
    public class EntityStateConfigCustomEditor : ExtendedInspector
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceID, int line)
        {
            if(enableInspectors)
            {
                EntityStateConfiguration obj = EditorUtility.InstanceIDToObject(instanceID) as EntityStateConfiguration;
                if (obj != null)
                {
                    ExtendedEditorWindow.OpenEditorWindow<EntityStateConfigEditorWindow>(obj, "Entity State Configuration Editor");
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
                ExtendedEditorWindow.OpenEditorWindow<EntityStateConfigEditorWindow>(target, "Entity State Configuration Editor");
            }
        }
    }
}
