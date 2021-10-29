using RoR2EditorKit.Settings;
using UnityEditor;

namespace RoR2EditorKit.Core.Inspectors
{
    public class ExtendedInspector : Editor
    {
        public RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }
        public static bool enableInspectors = true;

        public override void OnInspectorGUI()
        {
            enableInspectors = Settings.EditorWindowsEnabled;
            if (!enableInspectors)
            {
                DrawDefaultInspector();
            }
        }
    }
}
