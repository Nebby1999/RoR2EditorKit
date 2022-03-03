using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;
using RoR2EditorKit.Core.Inspectors;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(RoR2.BuffDef))]
    public class BuffDefInspector : ScriptableObjectInspector<BuffDef>
    {
        protected override VisualElement DrawInspectorGUI()
        {
            VisualElement element = new VisualElement();

            var header = RootVisualElement.Q<VisualElement>("Header");
            element.Add(header);

            var buffName = header.Q<Label>("buffDefName");
            buffName.BindProperty(serializedObject.FindProperty("m_Name"));

            var inspectorData = RootVisualElement.Q<VisualElement>("InspectorData");
            element.Add(inspectorData);

            var iconSprite = inspectorData.Q<ObjectField>("IconSprite");
            iconSprite.objectType = typeof(Sprite);
            iconSprite.BindProperty(serializedObject.FindProperty("iconSprite"));

            var buffColor = inspectorData.Q<ColorField>("BuffColor");
            buffColor.BindProperty(serializedObject.FindProperty("buffColor"));

            var eliteDef = inspectorData.Q<ObjectField>("EliteDef");
            eliteDef.objectType = typeof(EliteDef);
            eliteDef.BindProperty(serializedObject.FindProperty("eliteDef"));

            var startSFX = inspectorData.Q<ObjectField>("StartSFX");
            startSFX.objectType = typeof(NetworkSoundEventDef);
            startSFX.BindProperty(serializedObject.FindProperty("startSfx"));

            var canStack = inspectorData.Q<Toggle>("CanStack");
            canStack.BindProperty(serializedObject.FindProperty("canStack"));

            var isDebuff = inspectorData.Q<Toggle>("IsDebuff");
            canStack.BindProperty(serializedObject.FindProperty("isCooldown"));

            var isHidden = inspectorData.Q<Toggle>("IsHidden");
            canStack.BindProperty(serializedObject.FindProperty("isHidden"));

            return element;
        }
    }
}