using RoR2EditorKit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.PropertyDrawers
{
    /// <summary>
    /// Base class for creating a VisualElement property drawer
    /// </summary>
    public abstract class VisualElementPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// The root visual element of the property.
        /// This gets returneed by CreatePropertyGUI
        /// </summary>
        protected VisualElement RootVisualElement
        {
            get
            {
                if (_rootVisualElement == null)
                    _rootVisualElement = new VisualElement();
                return _rootVisualElement;
            }
        }

        private VisualElement _rootVisualElement;

        /// <summary>
        /// The serializedProperty that belongs to this property drawer
        /// </summary>
        protected SerializedProperty serializedProperty;

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _ = RootVisualElement;
            serializedProperty = property;
            DrawPropertyGUI();
            RootVisualElement.RegisterCallback<DetachFromPanelEvent>((_) => RootVisualElement.Wipe());
            return RootVisualElement;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.HelpBox(position, $"The VisualElementPropertyDrawer (which {GetType().Name} inherits from) does not support IMGUI." +
                $"\nIf this box appears on an Inspector that can be enabled or disabled, please keep it enabled." +
                $"\nIf you think this is an error, submit a bug report at the github's issue page.", MessageType.Info);
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Draw your property drawer here using UIToolkit
        /// </summary>
        protected abstract void DrawPropertyGUI();
    }
}
