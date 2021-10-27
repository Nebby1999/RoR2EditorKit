using HG.GeneralSerializer;
using RoR2;
using RoR2.Skills;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit
{
    public class EntityStateConfigEditorWindow : ExtendedEditorWindow
    {
        Vector2 scrollPos = new Vector2();


        private void OnGUI()
        {
            var collectionProperty = mainSerializedObject.FindProperty("serializedFieldsCollection");
            var systemTypeProp = mainSerializedObject.FindProperty("targetType");

            mainCurrentProperty = collectionProperty.FindPropertyRelative("serializedFields");

            EditorGUILayout.PropertyField(systemTypeProp);
            if(mainCurrentProperty.arraySize == 0)
            {
                if(SimpleButton("Auto populate fields"))
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
            var typeName = serializableSystemTypeProperty.FindPropertyRelative("assemblyQualifiedName").stringValue;
            Type type = Type.GetType(typeName);
            if(type != null)
            {
                FieldInfo[] fieldsToSerialize = Array.Empty<FieldInfo>();

                IEnumerable<FieldInfo> publicStaticFields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                publicStaticFields.Union(type.GetFields(BindingFlags.Default).Where(fieldInfo => fieldInfo.GetCustomAttribute<SerializeField>() != null));

                fieldsToSerialize = publicStaticFields.ToArray();

                for(int i = 0; i < fieldsToSerialize.Length; i++)
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