using UnityEditor;

namespace RoR2EditorKit.Core.Inspectors
{
    /// <summary>
    /// Inherit from this class to make your own Scriptable Object Inspectors.
    /// </summary>
    public abstract class ScriptableObjectInspector : ExtendedInspector
    {
        private bool drawnToggle = false;
        private void OnEnable()
        {
            InspectorEnabled = InspectorSetting.isEnabled;
            finishedDefaultHeaderGUI += DrawEnableToggle;
        }
        private void OnDisable() => finishedDefaultHeaderGUI -= DrawEnableToggle;

        private void DrawEnableToggle(Editor obj)
        {
            if (obj is ScriptableObjectInspector soInspector && !drawnToggle)
            {
                drawnToggle = true;
                InspectorEnabled = CreateEnableInsepctorToggle();
            }
        }
    }
}