using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderKit.Core.Data;
using UnityEditor;
using UnityEngine.Experimental.UIElements;
using ThunderKit.Markdown;
using UnityEditor.Experimental.UIElements;
using RoR2EditorKit.Common;

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

        public override void Initialize() => TokenPrefix = "";

        public override void CreateSettingsUI(VisualElement rootElement)
        {
            MarkdownElement markdown = null;
            if(string.IsNullOrEmpty(TokenPrefix))
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

            var child = CreateStandardField(nameof(EditorWindowsEnabled));
            child.tooltip = $"Uncheck this to disable the {Constants.RoR2EditorKit} custom inspectors";
            rootElement.Add(child);

            if (ror2EditorKitSettingsSO == null)
                ror2EditorKitSettingsSO = new SerializedObject(this);

            rootElement.Bind(ror2EditorKitSettingsSO);
        }
    }
}
