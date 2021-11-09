using RoR2EditorKit.Settings;
using UnityEditor;
using System.Linq;
using UnityEngine;

namespace RoR2EditorKit.Core.Inspectors
{
    public abstract class ExtendedInspector : Editor
    {
        public static RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }

        public RoR2EditorKitSettings.InspectorSetting InspectorSetting
        {
            get
            {
                if(_inspectorSetting == null)
                {
                    var setting = Settings.GetOrCreateInspectorSetting(GetType());
                    _inspectorSetting = setting;
                }
                return _inspectorSetting;
            }
            set
            {
                if(_inspectorSetting != value)
                {
                    var index = Settings.EnabledInspectors.IndexOf(_inspectorSetting);
                    Settings.EnabledInspectors[index] = value;
                }
                _inspectorSetting = value;
            }
        }

        private RoR2EditorKitSettings.InspectorSetting _inspectorSetting;

        public bool InspectorEnabled { get => InspectorSetting.isEnabled; set => InspectorSetting.isEnabled = value; }

        private void OnEnable()
        {
            InspectorEnabled = InspectorSetting.isEnabled;
        }
        public override void OnInspectorGUI()
        {
            InspectorEnabled = EditorGUILayout.Toggle("Enable Inspector", InspectorEnabled);
            GUILayout.Space(10);
            if (!InspectorEnabled)
            {
                DrawDefaultInspector();
            }
        }
    }
}
