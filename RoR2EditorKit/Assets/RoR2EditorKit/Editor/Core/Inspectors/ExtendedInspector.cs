using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using RoR2EditorKit.Settings;
using RoR2EditorKit.Common;

namespace RoR2EditorKit.Core.Inspectors
{
    using static ThunderKit.Core.UIElements.TemplateHelpers;

    public abstract class ExtendedInspector<T> : Editor where T : Object
    {
        public static RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }

        public EditorInspectorSettings.InspectorSetting InspectorSetting
        {
            get
            {
                if(_inspectorSetting == null)
                {
                    var setting = Settings.InspectorSettings.GetOrCreateInspectorSetting(GetType());
                    _inspectorSetting = setting;
                }
                return _inspectorSetting;
            }
            set
            {
                if(_inspectorSetting != value)
                {
                    var index = Settings.InspectorSettings.inspectorSettings.IndexOf(_inspectorSetting);
                    Settings.InspectorSettings.inspectorSettings[index] = value;
                }
                _inspectorSetting = value;
            }
        }

        private EditorInspectorSettings.InspectorSetting _inspectorSetting;

        public bool InspectorEnabled
        {
            get
            {
                return InspectorSetting.isEnabled;
            }
            set
            {
                if(value != InspectorSetting.isEnabled)
                {
                    InspectorSetting.isEnabled = value;
                    OnInspectorEnabledChange();
                }
            }
        }

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

        protected T targetType;
        protected VisualTreeAsset visualTreeAsset;

        protected virtual void OnEnable()
        {
        }
        private void OnInspectorEnabledChange()
        {
            RootVisualElement.Clear();
            RootVisualElement.styleSheets.Clear();
            OnRootElementCleared();

            if(!InspectorEnabled)
            {
                RootVisualElement.Add(new IMGUIContainer(OnInspectorGUI));
            }
            else
            {
                GetTemplateInstance(GetType().Name, RootVisualElement, IsFromRoR2EK);
                RootVisualElement.Bind(serializedObject);

                bool IsFromRoR2EK(string path)
                {
                    if (path.Contains(Constants.RoR2EditorKit))
                    {
                        return true;
                    }
                    return false;
                }
                RootVisualElement.Add(DrawInspectorGUI());
            }
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnRootElementCleared() { }
        public override VisualElement CreateInspectorGUI()
        {
            _ = RootVisualElement;
            OnInspectorEnabledChange();
            return RootVisualElement;
        }
        protected abstract VisualElement DrawInspectorGUI();
    }
}
