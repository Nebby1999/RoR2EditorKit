using RoR2EditorKit.Settings;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.Core.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(EnabledAndDisabledInspectorsSettings.InspectorSetting))]
    public class InspectorSettingPropertyDrawer : ExtendedPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Begin(position, label, property);
            
            var isEnabled = property.FindPropertyRelative("isEnabled");
            var displayName = property.FindPropertyRelative("inspectorName");

            GUIContent content = new GUIContent(NicifyName(displayName.stringValue), "Wether or not this inspector is enabled.");

            EditorGUI.PropertyField(position, isEnabled, content, false);

            End();
        }
    }
}
