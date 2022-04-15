using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.Core.PropertyDrawers
{
    /// <summary>
    /// Base class for creating IMGUI Property Drawers
    /// </summary>
    public abstract class IMGUIPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// The Rect of this Property Drawer
        /// </summary>
        public Rect rect;
        /// <summary>
        /// The Label of this Property Drawer
        /// </summary>
        public GUIContent label;
        /// <summary>
        /// The SerializedProperty tied to this PropertyDrawer
        /// </summary>
        public SerializedProperty property;

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            rect = position;
            this.label = label;
            this.property = property;

            DrawCustomDrawer();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float num = 16;
            foreach (var prop in property)
            {
                num += 16;
            }
            return num;
        }

        public float GetDefaultPropertyHeight()
        {
            return base.GetPropertyHeight(property, label);
        }

        protected abstract void DrawCustomDrawer();
        protected GUIContent Begin() => EditorGUI.BeginProperty(rect, label, property);
        protected void End() => EditorGUI.EndProperty();
        protected string NicifyName(string name) => ObjectNames.NicifyVariableName(name);
    }
}
