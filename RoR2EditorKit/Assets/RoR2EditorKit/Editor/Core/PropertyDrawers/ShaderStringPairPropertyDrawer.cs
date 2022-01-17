using UnityEngine;
using UnityEditor;
using RoR2EditorKit.Settings;

namespace RoR2EditorKit.Core.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(MaterialEditorSettings.ShaderStringPair))]
    public class ShaderStringPairPropertyDrawer : ExtendedPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Begin(position, label, property);
            var objRefProperty = property.FindPropertyRelative("shader");
            objRefProperty.objectReferenceValue = EditorGUI.ObjectField(position, NicifyName(property.FindPropertyRelative("shaderName").stringValue), objRefProperty.objectReferenceValue, typeof(Shader), false);
            End();
        }
    }
}
