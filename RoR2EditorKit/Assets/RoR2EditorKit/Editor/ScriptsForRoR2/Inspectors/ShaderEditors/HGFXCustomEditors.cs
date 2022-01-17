using UnityEditor;
using System;
using RoR2EditorKit.Core.Inspectors;
using BlendMode = UnityEngine.Rendering.BlendMode;
using static RoR2EditorKit.Core.Inspectors.ExtendedMaterialInspector;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    public static class HGFXCustomEditors
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            /*if (MaterialEditorEnabled)
                AddShader("hgCloudRemap", HGCloudRemapEditor, typeof(HGFXCustomEditors));*/
        }

        public static void HGCloudRemapEditor()
        {
            DrawBlendEnumProperty(GetProperty("_SrcBlend"));
            DrawBlendEnumProperty(GetProperty("_DstBlend"));
        }

        private static void DrawBlendEnumProperty(MaterialProperty prop)
        {
            float value = prop.floatValue;
            prop.floatValue = Convert.ToSingle(EditorGUILayout.EnumPopup(prop.displayName, (BlendMode)prop.floatValue));
        }
    }
}