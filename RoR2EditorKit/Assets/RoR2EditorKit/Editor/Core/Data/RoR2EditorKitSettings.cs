using RoR2EditorKit.Common;
using ThunderKit.Core.Data;
using ThunderKit.Markdown;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;

namespace RoR2EditorKit.Settings
{
    public class RoR2EditorKitSettings : ThunderKitSetting
    {
        const string MarkdownStylePath = "Packages/com.passivepicasso.thunderkit/Documentation/uss/markdown.uss";
        const string DocumentationStylePath = "Packages/com.passivepicasso.thunderkit/uss/thunderkit_style.uss";

        [InitializeOnLoadMethod]
        static void SetupSettings()
        {
            GetOrCreateSettings<RoR2EditorKitSettings>();
        }

        private SerializedObject ror2EditorKitSettingsSO;

        public string TokenPrefix;

        public bool EditorWindowsEnabled = true;

        public bool CloseWindowWhenAssetIsCreated = true;

        public override void Initialize() => TokenPrefix = "";

        public override void CreateSettingsUI(VisualElement rootElement)
        {
            MarkdownElement markdown = null;
            if (string.IsNullOrEmpty(TokenPrefix))
            {
                markdown = new MarkdownElement
                {
                    Data = $@"**__Warning:__** No Token Prefix assigned. Assign a token prefix before continuing.",

                    MarkdownDataType = MarkdownDataType.Text
                };
                markdown.AddStyleSheetPath(MarkdownStylePath);

                markdown.AddToClassList("m4");
                markdown.RefreshContent();
                rootElement.Add(markdown);
            }

            rootElement.Add(CreateStandardField(nameof(TokenPrefix)));

            var enableEditorWindows = CreateStandardField(nameof(EditorWindowsEnabled));
            enableEditorWindows.tooltip = $"Uncheck this to disable the {Constants.RoR2EditorKit} custom inspectors";
            rootElement.Add(enableEditorWindows);

            var assetCreatorCloses = CreateStandardField(nameof(CloseWindowWhenAssetIsCreated));
            assetCreatorCloses.tooltip = $"By default, when an asset creator window creates an asset, it closes, uncheck this so it doesnt close.";
            rootElement.Add(assetCreatorCloses);

            if (ror2EditorKitSettingsSO == null)
                ror2EditorKitSettingsSO = new SerializedObject(this);

            rootElement.Bind(ror2EditorKitSettingsSO);
        }
    }
}
