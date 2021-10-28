using RoR2;
using RoR2EditorKit.Core.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using RoR2EditorKit.Common;
using UnityEngine;
using RoR2EditorKit.Core;

namespace RoR2EditorKit.RoR2.EditorWindows
{
    class CreateArtifactDefWindow : CreateRoR2AssetWindow<ArtifactDef>
    {
        public ArtifactDef artifactDef;

        private string nameField;
        private bool createPickupPrefab;

        private string actualName;

        [MenuItem(Constants.RoR2EditorKitContextRoot + "ArtifactDef", false, Constants.RoR2EditorKitContextPriority)]
        public static void Open()
        {
            OpenEditorWindow<CreateArtifactDefWindow>(null, "Create Item");
        }

        protected override void OnWindowOpened()
        {
            base.OnWindowOpened();

            artifactDef = (ArtifactDef)scriptableObject;
            mainSerializedObject = new SerializedObject(artifactDef);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box");

            nameField = EditorGUILayout.TextField("Artifact Name", nameField);

            createPickupPrefab = EditorGUILayout.Toggle("Create Pickup Prefab", createPickupPrefab);

            if(GUILayout.Button("Create Artifact"))
            {
                var result = CreateArtifact();
                if(result)
                {
                    Debug.Log($"Succesfully Created Artifact {nameField}");
                    Close();
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            ApplyChanges();
        }

        private bool CreateArtifact()
        {
            actualName = GetCorrectAssetName(nameField);
            try
            {
                if (string.IsNullOrEmpty(actualName))
                    throw ErrorShorthands.ThrowNullAssetName(nameof(nameField));

                artifactDef.cachedName = actualName;

                if (string.IsNullOrEmpty(Settings.TokenPrefix))
                    throw ErrorShorthands.ThrowNullTokenPrefix();

                var tokenPrefix = $"{Settings.TokenPrefix}_ARTIFACT_{actualName.ToUpperInvariant()}_";
                artifactDef.nameToken = tokenPrefix + "NAME";
                artifactDef.descriptionToken = tokenPrefix + "DESC";

                artifactDef.pickupModelPrefab = createPickupPrefab ? CreatePickupPrefab() : null;

                Util.CreateAssetAtSelectionPath(artifactDef);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while creating artifact: {e}");
                return false;
            }
        }
        private GameObject CreatePickupPrefab()
        {
            var pickup = new GameObject($"Pickup{actualName}");
            var mdl = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mdl.name = $"mdl{actualName}";

            mdl.transform.AddTransformToParent(pickup.transform);

            var boxCollider = mdl.GetComponent<BoxCollider>();
            DestroyImmediate(boxCollider);

            return Util.CreatePrefabAtSelectionPath(pickup);
        }
    }
}
