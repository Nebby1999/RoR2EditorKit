using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using RoR2EditorKit.Common;
using RoR2EditorKit.Settings;

namespace RoR2EditorKit.Core.Windows
{
    public abstract class CreateRoR2AssetWindow<T> : ExtendedEditorWindow where T : ScriptableObject
    {
        public T scriptableObject { get; private set; }
        public RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }
        protected override void OnWindowOpened()
        {
            scriptableObject = ScriptableObject.CreateInstance<T>();
        }

        protected string GetCorrectAssetName(string name)
        {
            if(name.Contains(' '))
            {
                string[] strings = name.Split(' ');

                for(int i = 0; i < strings.Length; i++)
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
    }
}
