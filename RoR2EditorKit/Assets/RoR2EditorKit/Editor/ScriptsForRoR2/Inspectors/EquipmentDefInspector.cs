using RoR2;
using RoR2EditorKit.Core.Inspectors;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;
using System.Collections;
using Object = UnityEngine.Object;
using System.Collections.Generic;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(EquipmentDef))]
    public class EquipmentDefInspector : ScriptableObjectInspector<EquipmentDef>
    {
        GameObject pickupModel;
        IMGUIContainer modelMsg;
        float cooldown;
        IMGUIContainer cooldownMsg;
        float dropOnDeathChance;
        FloatField field;
        BuffDef passiveBuffDef;
        List<IMGUIContainer> bufDefMessages = new List<IMGUIContainer>();
        bool DoesNotAppear => (!TargetType.appearsInMultiPlayer && !TargetType.appearsInSinglePlayer);
        IMGUIContainer notAppearMessage;

        VisualElement header = null;
        VisualElement inspectorData = null;
        VisualElement messages = null;

        VisualElement slider = null;

        Button objectNameSetter;
        protected override void OnEnable()
        {
            base.OnEnable();
            pickupModel = TargetType.pickupModelPrefab;
            cooldown = TargetType.cooldown;
            dropOnDeathChance = TargetType.dropOnDeathChance;
            passiveBuffDef = TargetType.passiveBuffDef;

            OnVisualTreeCopy += () =>
            {
                header = Find<VisualElement>("Header");
                inspectorData = Find<VisualElement>("InspectorData");
                messages = Find<VisualElement>("Messages");
                Find<Toggle>("canBeRandomlyTriggered").Q<VisualElement>("");
                slider = Find<Slider>("dropOnDeathChance");
            };
        }
        protected override void DrawInspectorGUI()
        {

        }
    }
}