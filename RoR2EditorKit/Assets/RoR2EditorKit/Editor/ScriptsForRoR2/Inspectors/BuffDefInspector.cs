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

            FindAndBind<Label>(header, "m_Name");

            var inspectorData = RootVisualElement.Q<VisualElement>("InspectorData");
            element.Add(inspectorData);

            var iconSprite = FindAndBind<ObjectField>(inspectorData, "iconSprite");
            SetObjectType<Sprite>(iconSprite);

            FindAndBind<ColorField>(inspectorData, "buffColor");

            var eliteDef = FindAndBind<ObjectField>(inspectorData, "eliteDef");
            SetObjectType<EliteDef>(eliteDef);

            var startSfx = FindAndBind<ObjectField>(inspectorData, "startSfx");
            SetObjectType<NetworkSoundEventDef>(startSfx);

            FindAndBind<Toggle>(inspectorData, "canStack");
            FindAndBind<Toggle>(inspectorData, "isDebuff");
            FindAndBind<Toggle>(inspectorData, "isCooldown");
            FindAndBind<Toggle>(inspectorData, "isHidden");

            return element;
        }
    }
}