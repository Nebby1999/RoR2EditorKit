using RoR2EditorKit.Settings;
using System.Linq;
using UnityEngine;

namespace RoR2EditorKit.Core.Windows
{
    public abstract class CreateRoR2ScriptableObjectWindow<T> : ExtendedEditorWindow where T : ScriptableObject
    {
        public T scriptableObject { get; private set; }
        public RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }

        protected string nameField;
        protected string actualName;
        protected override void OnWindowOpened()
        {
            scriptableObject = ScriptableObject.CreateInstance<T>();
            nameField = string.Empty;
            actualName = string.Empty;
        }

        protected string GetCorrectAssetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            if (name.Contains(' '))
            {
                string[] strings = name.Split(' ');

                for (int i = 0; i < strings.Length; i++)
                {
                    strings[i] = char.ToUpper(strings[i][0]) + strings[i].Substring(1);
                }
                name = string.Join("", strings);
            }
            if (char.IsLower(name[0]))
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
            }
            return name;
        }

        protected void TryToClose()
        {
            if (Settings.CloseWindowWhenAssetIsCreated)
                Close();
            else
            {
                OnWindowOpened();
            }
        }
    }
}
