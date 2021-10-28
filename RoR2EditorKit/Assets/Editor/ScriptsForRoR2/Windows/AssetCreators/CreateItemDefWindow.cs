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
        private string actualName;

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
            actualName = GetCorrectAssetName(nameField);
            try
            {
                if (string.IsNullOrEmpty(actualName))
                    throw new NullReferenceException($"Field {nameField} cannot be Empty or null.");

                itemDef.name = actualName;

                if (string.IsNullOrEmpty(Settings.TokenPrefix))
                    throw new NullReferenceException($"Your TokenPrefix in the RoR2EditorKit settings is null or empty.");

                var tokenPrefix = $"{Settings.TokenPrefix}_ITEM_{actualName.ToUpperInvariant()}_";
                itemDef.nameToken = tokenPrefix + "NAME";
                itemDef.pickupToken = tokenPrefix + "PICKUP";
                itemDef.descriptionToken = tokenPrefix + "DESC";
                itemDef.loreToken = tokenPrefix + "LORE";

                itemDef.pickupModelPrefab = createPickupPrefab ? CreatePickupPrefab() : null;

                if (createItemDisplayPrefab)
                    CreateDisplayPrefab();

                Util.CreateAssetAtSelectionPath(itemDef);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while creating item: {e}");
                return false;
            }
        }

        private GameObject CreateDisplayPrefab()
        {
            //Creates game objects
            var display = new GameObject($"Display{actualName}");
            var mdl = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mdl.name = $"mdl{actualName}";

            //Parents mdl rpefab to parentPrefab
            mdl.transform.AddTransformToParent(display.transform);

            //Destroy uneeded components from mdl prefab
            var boxCollider = mdl.GetComponent<BoxCollider>();
            DestroyImmediate(boxCollider);

            //Add ItemDisplay component to parent prefab
            var itemDisplay = display.AddComponent<ItemDisplay>();
            var meshRenderer = mdl.GetComponent<MeshRenderer>();

            Array.Resize(ref itemDisplay.rendererInfos, 1);
            itemDisplay.rendererInfos[0].defaultMaterial = meshRenderer.sharedMaterial;
            itemDisplay.rendererInfos[0].renderer = meshRenderer;

            return Util.CreatePrefabAtSelectionPath(display);
        }
        private GameObject CreatePickupPrefab()
        {
            //Create game objects
            var pickup = new GameObject("Pickup" + actualName);
            var mdl = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mdl.name = "mdl" + actualName;

            //Parents prefabs
            mdl.transform.AddTransformToParent(pickup.transform);

            //Destroy box collider
            var boxCollider = mdl.GetComponent<BoxCollider>();
            DestroyImmediate(boxCollider);

            return Util.CreatePrefabAtSelectionPath(pickup);
        }
    }
}
