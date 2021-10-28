using System.Collections;
using System.Collections.Generic;
using ThunderKit.Core.Windows;
using UnityEditor;
using UnityEngine;
using RoR2EditorKit.Common;
using UnityEngine.Experimental.UIElements;
using ThunderKit.Core.Data;
using System.Linq;

namespace RoR2EditorKit.Settings
{
    using static ThunderKit.Core.UIElements.TemplateHelpers;
    public class RoR2EditorKitSettingsWindow : TemplatedWindow
    {
        readonly static string[] searchFolders = new[] { "Assets", "Packages" };

        [MenuItem(Constants.RoR2EditorKitMenuRoot + "Settings")]
        public static void ShowSettings()
        {
            GetWindow<RoR2EditorKitSettingsWindow>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            var settingsArea = rootVisualElement.Q("settings-area");
            var settingsPaths = AssetDatabase.FindAssets($"t:{nameof(ThunderKitSetting)}", searchFolders)
                .Select(AssetDatabase.GUIDToAssetPath).ToArray();

            foreach (var settingPath in settingsPaths)
            {
                var setting = AssetDatabase.LoadAssetAtPath<ThunderKitSetting>(settingPath);
                if (!setting)
                {
                    AssetDatabase.DeleteAsset(settingPath);
                    continue;
                }
                var settingSection = GetTemplateInstance("ThunderKitSettingSection");
                var title = settingSection.Q<Label>("title");
                if (title != null)
                    title.text = setting.name;
                var properties = settingSection.Q<VisualElement>("properties");
                try
                {
                    setting.CreateSettingsUI(properties);
                }
                catch
                {
                    var errorLabel = new Label($"Failed to load settings user interface");
                    errorLabel.AddToClassList("thunderkit-error");
                    properties.Add(errorLabel);
                }
                settingsArea.Add(settingSection);
            }
        }
    }
}
