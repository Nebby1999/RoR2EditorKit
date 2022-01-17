using UnityEditor;
using static RoR2EditorKit.Core.Inspectors.ExtendedMaterialInspector;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    public static class HGDeferredCustomEditors
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if(MaterialEditorEnabled)
                AddShader("hgStandard", HGStandardEditor, typeof(HGDeferredCustomEditors));
        }

        public static void HGStandardEditor()
        {
            DrawProperty("_EnableCutout");
            DrawProperty("_Color");
            DrawProperty("_MainTex");
            DrawProperty("_NormalStrength");
            DrawProperty("_NormalTex");
            DrawProperty("_EmColor");
            DrawProperty("_EmTex");
            DrawProperty("_EmPower");
            DrawProperty("_Smoothness");
            DrawProperty("_ForceSpecOn");
            DrawProperty("_RampInfo");
            DrawProperty("_DecalLayer");
            DrawProperty("_SpecularStrength");
            DrawProperty("_SpecularExponent");
            DrawProperty("_Cull");

            var prop = DrawProperty("_DitherOn");
            if(ShaderKeyword(prop))
            {
                DrawProperty("_FadeBias");
            }

            prop = DrawProperty("_FEON");
            if(ShaderKeyword(prop))
            {
                DrawProperty("_FresnelRamp");
                DrawProperty("_FresnelPower");
                DrawProperty("_FresnelMask");
                DrawProperty("_FresnelBoost");
            }

            prop = DrawProperty("_PrintOn");
            if(ShaderKeyword(prop))
            {
                DrawProperty("_SliceHeight");
                DrawProperty("_SliceBandHeight");
                DrawProperty("_SliceAlphaDepth");
                DrawProperty("_SliceAlphaTex");
                DrawProperty("_PrintBoost");
                DrawProperty("_PrintEmissionToAlbedoLerp");
                DrawProperty("_PrintDirection");
                DrawProperty("_PrintRamp");
            }

            Header("Elite Ramp");
            DrawProperty("_EliteBrightnessMin");
            DrawProperty("_EliteBrightnessMax");

            prop = DrawProperty("_SplatmapOn");
            if(ShaderKeyword(prop))
            {
                DrawProperty("_ColorsOn");
                DrawProperty("_Depth");
                DrawProperty("_SplatmapTex");
                DrawProperty("_SplatmapTileScale");
                DrawProperty("_GreenChannelTex");
                DrawProperty("_GreenChannelNormalTex");
                DrawProperty("_GreenChannelSmoothness");
                DrawProperty("_GreenChannelBias");
                DrawProperty("_BlueChannelTex");
                DrawProperty("_BlueChannelNormalTex");
                DrawProperty("_BlueChannelSmoothness");
                DrawProperty("_BlueChannelBias");
            }

            prop = DrawProperty("_FlowmapOn");
            if(ShaderKeyword(prop))
            {
                DrawProperty("_FlowTex");
                DrawProperty("_FlowHeightmap");
                DrawProperty("_FlowHeightRamp");
                DrawProperty("_FlowHeightBias");
                DrawProperty("_FlowHeightPower");
                DrawProperty("_FlowEmissionStrength");
                DrawProperty("_FlowSpeed");
                DrawProperty("_FlowMaskStrength");
                DrawProperty("_FlowNormalStrength");
                DrawProperty("_FlowTextureScaleFactor");
            }

            DrawProperty("_LimbRemovalOn");
        }
    }
}