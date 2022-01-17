using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ThunderKit.Core.Data;
using RoR2EditorKit.Settings;
using Object = UnityEngine.Object;

namespace RoR2EditorKit.Core.Inspectors
{
    [CustomEditor(typeof(Material))]
    public class ExtendedMaterialInspector : MaterialEditor
    {
        private static Dictionary<string, Action> shaderNameToAction = new Dictionary<string, Action>();

        public static RoR2EditorKitSettings Settings { get => ThunderKitSetting.GetOrCreateSettings<RoR2EditorKitSettings>(); }

        public static bool MaterialEditorEnabled { get => Settings.MaterialEditorSettings.EnableMaterialEditor; }

        public static MaterialEditor Instance { get; private set; }

        private Action chosenActionForMaterial = null;

        private Material material;

        public override void Awake()
        {
            base.Awake();
            if(MaterialEditorEnabled)
            {
                material = target as Material;
                GetActionForMaterial();
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            GetActionForMaterial();
        }

        private void GetActionForMaterial()
        {
            foreach(var shaderStringPair in Settings.MaterialEditorSettings.shaderStringPairs)
            {
                if (shaderNameToAction.ContainsKey(shaderStringPair.shaderName) && material.shader == shaderStringPair.shader)
                {
                    chosenActionForMaterial = shaderNameToAction[shaderStringPair.shaderName];
                    return;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            Instance = this as MaterialEditor;
            if (chosenActionForMaterial != null)
            {
                chosenActionForMaterial.Invoke();
                serializedObject.ApplyModifiedProperties();
            }
            else
                base.OnInspectorGUI();
        }

        public static void AddShader(string shaderName, Action inspectorForShader, Type callingType)
        {
            Settings.MaterialEditorSettings.CreateShaderStringPairIfNull(shaderName, callingType);

            shaderNameToAction.Add(shaderName, inspectorForShader);
        }

        public static MaterialProperty DrawProperty(string name)
        {
            if(Instance)
            {
                MaterialProperty prop = GetMaterialProperty(Instance.targets, name);
                Instance.ShaderProperty(prop, prop.displayName);
                return prop;
            }
            return null;
        }

        public static MaterialProperty GetProperty(string name)
        {
            if (Instance)
                return GetMaterialProperty(Instance.targets, name);
            return null;
        }

        public static bool ShaderKeyword(MaterialProperty prop)
        {
            if (prop.floatValue == 1)
                return true;
            else if (prop.floatValue == 0)
                return false;

            return false;
        }

        /// <summary>
        /// Creates a Header for the inspector
        /// </summary>
        /// <param name="label">The text for the label used in this header</param>
        public static void Header(string label) => EditorGUILayout.LabelField(new GUIContent(label), EditorStyles.boldLabel);

        /// <summary>
        /// Creates a Header with a tooltip for the inspector
        /// </summary>
        /// <param name="label">The text for the label used in this header</param>
        /// <param name="tooltip">A tooltip that's displayed after the mouse hovers over the label</param>
        public static void Header(string label, string tooltip) => EditorGUILayout.LabelField(new GUIContent(label, tooltip), EditorStyles.boldLabel);

    }
}
