using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.Core.Inspectors
{
    /// <summary>
    /// Inherit from this class to make your own Component Inspectors.
    /// </summary>
    public abstract class ComponentInspector<T> : ExtendedInspector<T> where T : MonoBehaviour
    {
        private Toggle inspectorEnabledToggle;
        protected override void OnEnable()
        {
            base.OnEnable();
            inspectorEnabledToggle = new Toggle($"Enable {ObjectNames.NicifyVariableName(target.GetType().Name)} Inspector");
            inspectorEnabledToggle.RegisterValueChangedCallback(OnToggleChanged);
        }

        protected override void OnRootElementCleared()
        {
            RootVisualElement.Add(inspectorEnabledToggle);
        }

        private void OnToggleChanged(ChangeEvent<bool> evt)
        {
            InspectorEnabled = evt.newValue;
        }
    }
}