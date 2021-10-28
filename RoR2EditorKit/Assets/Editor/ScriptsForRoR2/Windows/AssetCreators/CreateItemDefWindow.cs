using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;
using RoR2EditorKit.Common;
using RoR2EditorKit.Core.Windows;
using ThunderKit.Core;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.RoR2.EditorWindows
{
    public class CreateItemDefWindow : CreateRoR2AssetWindow<ItemDef>
    {
        public ItemDef itemDef;

        private string nameField;
        private bool createPickupPrefab;
        private bool createItemDisplayPrefab;

        [MenuItem(Constants.RoR2EditorKitContextRoot + "ItemDef", false, Constants.RoR2EditorKitContextPriority)]
        public static void Open()
        {
            OpenEditorWindow<CreateItemDefWindow>(null, "Create Item");
        }
        protected override void OnWindowOpened()
        {
            base.OnWindowOpened();

            itemDef = (ItemDef)scriptableObject;
            mainSerializedObject = new SerializedObject(itemDef);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box");

            nameField = EditorGUILayout.TextField("Item Name", nameField);

            DrawField(mainSerializedObject.FindProperty("tier"), true, "Item Tier");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.LabelField($"Item Tags");
            DrawValueSidebar(mainSerializedObject.FindProperty("tags"));
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            createItemDisplayPrefab = EditorGUILayout.Toggle("Create Display Prefab", createItemDisplayPrefab);
            createPickupPrefab = EditorGUILayout.Toggle("Create Pickup Prefab", createPickupPrefab);

            if(GUILayout.Button($"Create Item"))
            {
                var result = CreateItem();
                if(result)
                {
                    Debug.Log($"Succesfully Created Item {nameField}");
                    Close();
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            ApplyChanges();
        }

        private bool CreateItem()
        {
            try
            {
                CreateItemDef();

                if (createItemDisplayPrefab)
                    CreateDisplayPrefab();

                if (createPickupPrefab)
                    CreatePickupPrefab();

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while creating item: {e}");
                return false;
            }
        }
        private void CreateItemDef()
        {
            if (string.IsNullOrEmpty(nameField))
                throw new NullReferenceException($"Field {nameField} cannot be Empty or null.");

            var correctName = GetCorrectAssetName(nameField);

            itemDef.name = correctName;

            if (string.IsNullOrEmpty(Settings.TokenPrefix))
                throw new NullReferenceException($"Your TokenPrefix in the RoR2EditorKit settings is null or empty.");

            var tokenPrefix = $"{Settings.TokenPrefix}_ITEM_{correctName.ToUpperInvariant()}_";
            itemDef.nameToken = tokenPrefix + "NAME";
            itemDef.pickupToken = tokenPrefix + "PICKUP";
            itemDef.descriptionToken = tokenPrefix + "DESC";
            itemDef.loreToken = tokenPrefix + "LORE";

            Util.CreateAssetAtPath(itemDef);
        }

        private void CreateDisplayPrefab()
        {

        }
        private void CreatePickupPrefab()
        {

        }
    }
}
