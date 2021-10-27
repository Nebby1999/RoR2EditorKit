using RoR2.ContentManagement;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit
{
    class SerializableContentPackEditorWindow : ExtendedEditorWindow
    {
        Vector2 scrollPos = new Vector2();
        SerializableContentPack contentPack;
        string selectedArrayPath;

        private void OnGUI()
        {
            contentPack = mainSerializedObject.targetObject as SerializableContentPack;
            string[] fieldNames = contentPack.GetType()
                .GetFields()
                .Select(fieldInfo => fieldInfo.Name)
                .ToArray();

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(300), GUILayout.ExpandHeight(true));

            DrawButtonSidebar(fieldNames);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            if (mainSelectedProperty != null)
            {
                DrawSelectedArray();
            }
            else
            {
                EditorGUILayout.LabelField("Select an Content Element from the List.");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            ApplyChanges();
        }

        private void DrawButtonSidebar(string[] fieldNames)
        {
            foreach (string field in fieldNames)
            {
                if (GUILayout.Button(field))
                {
                    selectedArrayPath = mainSerializedObject.FindProperty(field).propertyPath;
                }
            }
            if (!string.IsNullOrEmpty(selectedArrayPath))
            {
                mainSelectedProperty = mainSerializedObject.FindProperty(selectedArrayPath);
            }
        }

        private void DrawSelectedArray()
        {
            mainCurrentProperty = mainSelectedProperty;

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(500));

            DrawValueSidebar(mainCurrentProperty);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}