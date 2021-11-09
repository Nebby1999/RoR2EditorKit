using RoR2.ContentManagement;
using RoR2EditorKit.Core.Windows;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.RoR2.EditorWindows
{
    class SerializableContentPackEditorWindow : ExtendedEditorWindow
    {
        Vector2 scrollPos = new Vector2();
        SerializableContentPack contentPack;
        SerializedProperty mainSelectedProp;
        SerializedProperty mainCurrentProp;
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
            if (mainSelectedProp != null)
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
                mainSelectedProp = mainSerializedObject.FindProperty(selectedArrayPath);
            }
        }

        private void DrawSelectedArray()
        {
            mainCurrentProp = mainSelectedProp;

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(500));

            DrawValueSidebar(mainCurrentProp);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}