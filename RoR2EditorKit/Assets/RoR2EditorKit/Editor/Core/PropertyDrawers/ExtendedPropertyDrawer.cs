using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.Core.PropertyDrawers
{
    public abstract class ExtendedPropertyDrawer : PropertyDrawer
    {
        protected GUIContent Begin(Rect totalPos, GUIContent label, SerializedProperty property) => EditorGUI.BeginProperty(totalPos, label, property);
        protected void End() => EditorGUI.EndProperty();
        protected string NicifyName(string name) => ObjectNames.NicifyVariableName(name);
    }
}
