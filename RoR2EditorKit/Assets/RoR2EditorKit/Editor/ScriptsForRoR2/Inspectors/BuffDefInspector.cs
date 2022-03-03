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
        private EliteDef eliteDef;
        private List<IMGUIContainer> eliteDefMessages = new List<IMGUIContainer>();

        private NetworkSoundEventDef networkSoundEventDef;
        private IMGUIContainer networkSoundEventdefMessage = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            eliteDef = serializedObject.FindProperty(nameof(eliteDef)).objectReferenceValue as EliteDef;
            networkSoundEventDef = serializedObject.FindProperty("startSfx").objectReferenceValue as NetworkSoundEventDef;
            prefix = "bd";
        }

        protected override VisualElement DrawInspectorGUI()
        {
            VisualElement element = new VisualElement();

            var header = Find<VisualElement>("Header");
            element.Add(header);

            var label = Find<Label>(header, "m_Name");
            label.RegisterValueChangedCallback((cb) => EnsureNamingConventions(cb));

            var inspectorData = Find<VisualElement>("InspectorData");
            element.Add(inspectorData);

            Find<ObjectField>(inspectorData, "iconSprite").SetObjectType<Sprite>();

            var eliteDef = Find<ObjectField>(inspectorData, "eliteDef");
            eliteDef.SetObjectType<EliteDef>();
            eliteDef.RegisterValueChangedCallback(CheckEliteDef);

            var startSfx = Find<ObjectField>(inspectorData, "startSfx");
            startSfx.SetObjectType<NetworkSoundEventDef>();
            startSfx.RegisterValueChangedCallback(CheckSoundEvent);

            return element;
        }

        protected override void OnDrawInspectorGUICalled()
        {
            CheckEliteDef();
            CheckSoundEvent();
        }

        private void CheckSoundEvent(ChangeEvent<UnityEngine.Object> evt = null)
        {
            if(networkSoundEventdefMessage != null)
            {
                networkSoundEventdefMessage.TryRemoveFromParent();
            }

            if (evt != null)
            {
                networkSoundEventDef = evt.newValue as NetworkSoundEventDef;
            }

            if (!networkSoundEventDef)
                return;

            if(networkSoundEventDef.eventName.IsNullOrEmptyOrWhitespace())
            {
                networkSoundEventdefMessage = CreateHelpBox($"You've associated a NetworkSoundEventDef ({networkSoundEventDef.name}) to this buff, but the EventDef's eventName is Null, Empty or Whitespace!", MessageType.Warning);
            }
        }

        private void CheckEliteDef(ChangeEvent<UnityEngine.Object> evt = null)
        {
            ColorField buffColor = Find<ColorField>("buffColor");
            Button button = Find<Button>(buffColor, "colorSetter");

            if (button != null)
                button.TryRemoveFromParent();

            foreach(IMGUIContainer container in eliteDefMessages)
            {
                if (container != null)
                    container.TryRemoveFromParent();
            }

            if(evt != null)
            {
                eliteDef = evt.newValue as EliteDef;
            }

            if (!eliteDef)
                return;

            button = new Button(() =>
            {
                buffColor.value = eliteDef.color;
            });
            button.name = "colorSetter";
            button.text = "Set color to Elite color";
            buffColor.Add(button);

            if(!eliteDef.eliteEquipmentDef)
            {
                eliteDefMessages.Add(CreateHelpBox($"You've associated an EliteDef ({eliteDef.name}) to this buff, but the EliteDef has no EquipmentDef assigned!", MessageType.Warning));
            }

            if(eliteDef.eliteEquipmentDef && !eliteDef.eliteEquipmentDef.passiveBuffDef)
            {
                eliteDefMessages.Add(CreateHelpBox($"You've associated an EliteDef ({eliteDef.name}) to this buff, but the assigned EquipmentDef ({eliteDef.eliteEquipmentDef.name})'s \"passiveBuffDef\" is not asigned!", MessageType.Warning));
            }

            if(eliteDef.eliteEquipmentDef && eliteDef.eliteEquipmentDef.passiveBuffDef != TargetType)
            {
                eliteDefMessages.Add(CreateHelpBox($"You've associated an EliteDef ({eliteDef.name}) to this buff, but the assigned EquipmentDef ({eliteDef.eliteEquipmentDef.name})'s \"passiveBuffDef\" is not the inspected BuffDef!", MessageType.Warning));
            }
        }

        /*private void CheckName(ChangeEvent<string> evt = null)
        {
            if (m_NameContainer != null)
            {
                m_NameContainer.TryRemoveFromParent();
            }

            if(evt != null)
            {
                m_Name.stringValue = evt.newValue;
            }

            if(!m_Name.stringValue.StartsWith("bd"))
            {
                m_NameContainer = CreateHelpBox("This buffDef's name should start with bd for naming conventions.", MessageType.Info);
                m_NameContainer.SendToBack();
            }
        }*/

        protected override IMGUIContainer EnsureNamingConventions(ChangeEvent<string> evt = null)
        {
            base.EnsureNamingConventions()?.SendToBack();
            return null;
        }
    }
}