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
            var header = RootVisualElement.Q<VisualElement>("Header");

            var buffName = RootVisualElement.Query().Descendents<Label>("BuffDefName").First();

            /*var iconSprite = QuickQ<ObjectField>("IconSprite");
            var buffColor = QuickQ<ColorField>("BuffColor");
            var eliteDef = QuickQ<ObjectField>("EliteDef");
            var startSFX = QuickQ<ObjectField>("StartSFX");
            var canStack = QuickQ<Toggle>("canStack");
            var isDebuff = QuickQ<Toggle>("IsDebuff");
            var isCooldown = QuickQ<Toggle>("IsCooldown");
            var isHidden = QuickQ<Toggle>("IsHidden");*/

            //buffName.text = targetType.name;
            return new Label("Lol");
        }
    }
}