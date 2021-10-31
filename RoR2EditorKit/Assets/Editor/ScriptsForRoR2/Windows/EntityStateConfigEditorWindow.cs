using HG.GeneralSerializer;
using RoR2EditorKit.Core.Windows;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.RoR2.EditorWindows
{
    public class EntityStateConfigEditorWindow : ExtendedEditorWindow
    {
        Vector2 scrollPos = new Vector2();

        Type entityStateType;

        private void OnGUI()
        {
            var collectionProperty = mainSerializedObject.FindProperty("serializedFieldsCollection");
            var systemTypeProp = mainSerializedObject.FindProperty("targetType");
            entityStateType = Type.GetType(systemTypeProp.FindPropertyRelative("assemblyQualifiedName").stringValue);

            mainCurrentProperty = collectionProperty.FindPropertyRelative("serializedFields");

            EditorGUILayout.PropertyField(systemTypeProp);
            if (mainCurrentProperty.arraySize == 0)
            {
                if (SimpleButton("Auto populate fields"))
                {
                    AutoPopulateFields(systemTypeProp);
                }
            }

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

            var tuple = DrawScrollableButtonSidebar(mainCurrentProperty, scrollPos);
            scrollPos = tuple.Item1;

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            if (mainSelectedProperty != null)
            {
                DrawSelectedSerializableFieldPropPanel();
            }
            else
            {
                EditorGUILayout.LabelField("Select a Serializable Field from the List.");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            ApplyChanges();
        }

        private void DrawSelectedSerializableFieldPropPanel()
        {
            mainCurrentProperty = mainSelectedProperty;

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(500));

            DrawField("fieldName", true);

            GUILayout.Space(30);

            var fieldValueProp = mainSelectedProperty.FindPropertyRelative("fieldValue");

            var stringValue = fieldValueProp.FindPropertyRelative("stringValue");
            var objValue = fieldValueProp.FindPropertyRelative("objectValue");

            if (!string.IsNullOrEmpty(stringValue.stringValue))
            {
                DrawField(stringValue, true);
            }
            else if (objValue.objectReferenceValue != null)
            {
                DrawField(objValue, true);
            }
            else if (objValue.objectReferenceValue == null && string.IsNullOrEmpty(stringValue.stringValue))
            {
                DrawField(stringValue, true);
                DrawField(objValue, true);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void AutoPopulateFields(SerializedProperty serializableSystemTypeProperty)
        {
            if (entityStateType != null)
            {
                var allFieldsInType = entityStateType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                var filteredFields = allFieldsInType.Where(fieldInfo =>
                {
                    bool canSerialize = SerializedValue.CanSerializeField(fieldInfo);
                    bool shouldSerialize = !fieldInfo.IsStatic || (fieldInfo.DeclaringType == entityStateType);
                    return canSerialize && shouldSerialize;
                });

                var staticFields = filteredFields.Where(fieldInfo => fieldInfo.IsStatic);
                var instanceFields = filteredFields.Where(fieldInfo => !fieldInfo.IsStatic);


                FieldInfo[] fieldsToSerialize = staticFields.Union(instanceFields).ToArray();

                for (int i = 0; i < fieldsToSerialize.Length; i++)
                {
                    mainCurrentProperty.arraySize = i + 1;
                    var serializedField = mainCurrentProperty.GetArrayElementAtIndex(i);
                    var fieldName = serializedField.FindPropertyRelative("fieldName");
                    fieldName.stringValue = fieldsToSerialize[i].Name;
                }
            }
        }
    }
}