using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using RoR2EditorKit.Settings;
using RoR2EditorKit.Common;
using System;
using Object = UnityEngine.Object;

namespace RoR2EditorKit.Core.Inspectors
{
    using static ThunderKit.Core.UIElements.TemplateHelpers;

    /// <summary>
    /// Base inspector for all the RoR2EditorKit Inspectors.
    /// <para>If you want to make a Scriptable Object Inspector, you'll probably want to use the ScriptableObjwectInspector</para>
    /// <para>If you want to make an Inspector for a Component, you'll probably want to use the ComponentInspector</para>
    /// </summary>
    /// <typeparam name="T">The type of Object being inspected</typeparam>
    public abstract class ExtendedInspector<T> : Editor where T : Object
    {
        /// <summary>
        /// Access to the Settings file
        /// </summary>
        public static RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }

        /// <summary>
        /// The setting for this inspector
        /// </summary>
        public EditorInspectorSettings.InspectorSetting InspectorSetting
        {
            get
            {
                if(_inspectorSetting == null)
                {
                    var setting = Settings.InspectorSettings.GetOrCreateInspectorSetting(GetType());
                    _inspectorSetting = setting;
                }
                return _inspectorSetting;
            }
            set
            {
                if(_inspectorSetting != value)
                {
                    var index = Settings.InspectorSettings.inspectorSettings.IndexOf(_inspectorSetting);
                    Settings.InspectorSettings.inspectorSettings[index] = value;
                }
                _inspectorSetting = value;
            }
        }

        private EditorInspectorSettings.InspectorSetting _inspectorSetting;

        /// <summary>
        /// Check if the inspector is enabled
        /// <para>If you're setting the value, and the value is different from the initial value, the inspector will redraw completely to accomodate the new look using either the base inspector or custom inspector</para>
        /// </summary>
        public bool InspectorEnabled
        {
            get
            {
                return InspectorSetting.isEnabled;
            }
            set
            {
                if(value != InspectorSetting.isEnabled)
                {
                    InspectorSetting.isEnabled = value;
                    OnInspectorEnabledChange();
                }
            }
        }

        /// <summary>
        /// The root visual element of the inspector
        /// </summary>
        protected VisualElement RootVisualElement
        {
            get
            {
                if (_rootVisualElement == null)
                    _rootVisualElement = new VisualElement();

                return _rootVisualElement;
            }
        }

        private VisualElement _rootVisualElement;

        /// <summary>
        /// Direct access to the object that's being inspected as its type.
        /// </summary>
        protected T TargetType { get; private set; }
        /// <summary>
        /// The visual tree asset, every inspector should have an UXML file with the inspector layout
        /// </summary>
        protected VisualTreeAsset visualTreeAsset;

        /// <summary>
        /// The prefix this asset should use, leave this null unless the asset youre creating requires a prefix.
        /// </summary>
        protected string prefix = null;
        private IMGUIContainer prefixContainer = null;

        private void GetTemplate()
        {
            GetTemplateInstance(GetType().Name, RootVisualElement, IsFromRoR2EK);
            RootVisualElement.Bind(serializedObject);

            bool IsFromRoR2EK(string path)
            {
                if (path.Contains(Constants.RoR2EditorKit))
                {
                    return true;
                }
                return false;
            }
        }
        private void OnInspectorEnabledChange()
        {
            RootVisualElement.Clear();
            RootVisualElement.styleSheets.Clear();
            OnRootElementCleared();
            EnsureNamingConventions();

            if(!InspectorEnabled)
            {
                RootVisualElement.Add(new IMGUIContainer(OnInspectorGUI));
            }
            else
            {
                GetTemplate();
                RootVisualElement.Add(DrawInspectorGUI());
                OnDrawInspectorGUICalled();
            }
            serializedObject.ApplyModifiedProperties();
        }
        /// <summary>
        /// Called when the inspector is enabled, always keep the original implementation unless you know what youre doing
        /// </summary>
        protected virtual void OnEnable()
        {
            TargetType = target as T;
            GetTemplate();
        }
        /// <summary>
        /// When the inspector initializes, or it's enabled setting changes, the RootVisualElement gets cleared, when this happens, this method gets run
        /// <para>use this method if you need to set up anything specific that should always appear, regardless if the custom inspector is enabled.</para>
        /// </summary>
        protected virtual void OnRootElementCleared() { }
        /// <summary>
        /// When the inspector initializes, and/or it's enabled setting is set to true, this method runs right after DrawInspectorGUI is called.
        /// <para>use this method if you need to set up anything specific that should always appear at the bottom.</para>
        /// </summary>
        protected virtual void OnDrawInspectorGUICalled() { }
        /// <summary>
        /// DO NOT OVERRIDE THIS METHOD. Use "DrawInspectorGUI" if you want to implement your inspector!
        /// </summary>
        /// <returns>DO NOT OVERRIDE THIS METHOD. Use "DrawInspectorGUI" if you want to implement your inspector!</returns>
        public override VisualElement CreateInspectorGUI()
        {
            _ = RootVisualElement;
            OnInspectorEnabledChange();
            serializedObject.ApplyModifiedProperties();
            return RootVisualElement;
        }
        /// <summary>
        /// Implement your inspector here
        /// </summary>
        /// <returns>The visual element that's going to be attached to the RootVisualElement</returns>
        protected abstract VisualElement DrawInspectorGUI();

        #region Util Methods
        protected TElement Find<TElement>(string name = null, string ussClass = null) where TElement : VisualElement
        {
            return RootVisualElement.Q<TElement>(name, ussClass);
        }
        protected TElement Find<TElement>(VisualElement elementToSearch, string name = null, string ussClass = null)where TElement : VisualElement
        {
            return elementToSearch.Q<TElement>(name, ussClass);
        }
        /// <summary>
        /// Queries a visual element of type T from the RootVisualElement, and binds it to a property on the serialized object.
        /// <para>Property is found by using the Element's name as the binding path</para>
        /// </summary>
        /// <typeparam name="TElement">The Type of VisualElement, must inherit IBindable</typeparam>
        /// <param name="name">Optional parameter to find the Element, used in the Quering</param>
        /// <param name="ussClass">Optional parameter of the name of a USSClass the element youre finding uses</param>
        /// <returns>The VisualElement specified, with a binding to the property</returns>
        protected TElement FindAndBind<TElement>(string name = null, string ussClass = null) where TElement : VisualElement, IBindable
        {
            var bindableElement = RootVisualElement.Q<TElement>(name, ussClass);
            if (bindableElement == null)
                throw new NullReferenceException($"Could not find element of type {typeof(TElement)} inside the RootVisualElement.");

            bindableElement.bindingPath = bindableElement.name;
            bindableElement.BindProperty(serializedObject);

            return bindableElement;
        }

        /// <summary>
        /// Queries a visual element of type T from the RootVisualElement, and binds it to a property on the serialized object.
        /// </summary>
        /// <typeparam name="TElement">The Type of VisualElement, must inherit IBindable</typeparam>
        /// <param name="prop">The property which is used in the Binding process</param>
        /// <param name="name">Optional parameter to find the Element, used in the Quering</param>
        /// <param name="ussClass">Optional parameter of the name of a USSClass the element youre finding uses</param>
        /// <returns>The VisualElement specified, with a binding to the property</returns>
        protected TElement FindAndBind<TElement>(SerializedProperty prop, string name = null, string ussClass = null) where TElement : VisualElement, IBindable
        {
            var bindableElement = RootVisualElement.Q<TElement>(name, ussClass);
            if (bindableElement == null)
                throw new NullReferenceException($"Could not find element of type {typeof(TElement)} inside the RootVisualElement.");

            bindableElement.BindProperty(prop);

            return bindableElement;
        }

        /// <summary>
        /// Queries a visual element of type T from the elementToSearch argument, and binds it to a property on the serialized object.
        /// <para>Property is found by using the Element's name as the binding path</para>
        /// </summary>
        /// <typeparam name="TElement">The Type of VisualElement, must inherit IBindable</typeparam>
        /// <param name="elementToSearch">The VisualElement where the Quering process will be done.</param>
        /// <param name="name">Optional parameter to find the Element, used in the Quering</param>
        /// <param name="ussClass">The name of a USSClass the element youre finding uses</param>
        /// <returns>The VisualElement specified, with a binding to the property</returns>
        protected TElement FindAndBind<TElement>(VisualElement elementToSearch, string name = null, string ussClass = null) where TElement : VisualElement, IBindable
        {
            var bindableElement = elementToSearch.Q<TElement>(name, ussClass);
            if (bindableElement == null)
                throw new NullReferenceException($"Could not find element of type {typeof(TElement)} inside element {elementToSearch.name}.");

            bindableElement.bindingPath = bindableElement.name;
            bindableElement.BindProperty(serializedObject);

            return bindableElement;
        }

        /// <summary>
        /// Queries a visual element of type T from the elementToSearch argument, and binds it to a property on the serialized object.
        /// <para>Property is found by using the Element's name as the binding path</para>
        /// </summary>
        /// <typeparam name="TElement">The Type of VisualElement, must inherit IBindable</typeparam>
        /// <param name="elementToSearch">The VisualElement where the Quering process will be done.</param>
        /// <param name="name">Optional parameter to find the Element, used in the Quering</param>
        /// <param name="ussClass">The name of a USSClass the element youre finding uses</param>
        /// <returns>The VisualElement specified, with a binding to the property</returns>
        protected TElement FindAndBind<TElement>(VisualElement elementToSearch, SerializedProperty prop, string name = null, string ussClass = null) where TElement : VisualElement, IBindable
        {
            var bindableElement = elementToSearch.Q<TElement>(name, ussClass);
            if (bindableElement == null)
                throw new NullReferenceException($"Could not find element of type {typeof(TElement)} inside element {elementToSearch.name}.");

            bindableElement.BindProperty(prop);

            return bindableElement;
        }

        /// <summary>
        /// Creates a HelpBox and attatches it to a visualElement using IMGUIContainer
        /// </summary>
        /// <param name="message">The message that'll appear on the help box</param>
        /// <param name="messageType">The type of message</param>
        /// <param name="elementToAttach">Optional, if specified, the Container will be added to this element, otherwise, it's added to the RootVisualElement</param>
        protected IMGUIContainer CreateHelpBox(string message, MessageType messageType, VisualElement elementToAttach = null)
        {
            IMGUIContainer container = new IMGUIContainer();
            container.onGUIHandler = () =>
            {
                EditorGUILayout.HelpBox(message, messageType);
            };

            if(elementToAttach != null)
            {
                elementToAttach.Add(container);
                return container;
            }
            RootVisualElement.Add(container);
            return container;
        }

        /// <summary>
        /// Ensure the naming convention for a specific object stays.
        /// <para>This method is ran right after OnRootElementCleared by default.</para>
        /// <para>Requires that the prefix for this inspector is not null.</para>
        /// </summary>
        /// <param name="evt">The ChangeEvent, used if the Method is used on a visual element's RegisterValueChange</param>
        /// <returns>If the convention is not followed, an IMGUIContainer with a help box, otherwise it returns null.</returns>
        protected virtual IMGUIContainer EnsureNamingConventions(ChangeEvent<string> evt = null)
        {
            if(!Settings.InspectorSettings.enableNamingConventions)
            {
                return null;
            }

            if(prefixContainer != null)
            {
                prefixContainer.TryRemoveFromParent();
            }

            if(evt != null)
            {
                TargetType.name = evt.newValue;
            }

            if(prefix != null)
            {
                if(TargetType && !TargetType.name.StartsWith(prefix))
                {
                    string typeName = typeof(T).Name;
                    prefixContainer = CreateHelpBox($"This {typeName}'s name should start with {prefix} for naming conventions.", MessageType.Info);
                    return prefixContainer;
                }
            }
            return null;
        }

        #endregion
    }

    [Obsolete("The " + nameof(LegacyExtendedInspectorGUILayoutMethods) + "class is going to be removed once all the default inspectors have been migrated to UIElements.")]
    public class LegacyExtendedInspectorGUILayoutMethods
    {
        public LegacyExtendedInspectorGUILayoutMethods(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
        }

        private SerializedObject serializedObject;
        /// <summary>
        /// Draws a property field using the given property name
        /// <para>The property will be found from the serialized object that's being inspected</para>
        /// </summary>
        /// <param name="propName">The property's name</param>
        public void DrawField(string propName) => EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), true);
        /// <summary>
        /// Draws a property field using the given property name
        /// <para>The property will be found from the given SerializedProperty</para>
        /// </summary>
        /// <param name="property">The property to search in</param>
        /// <param name="propName">The property to find and draw</param>
        public void DrawField(SerializedProperty property, string propName) => EditorGUILayout.PropertyField(property.FindPropertyRelative(propName), true);
        /// <summary>
        /// Draws a property field using the given property
        /// </summary>
        /// <param name="property">The property to draw</param>
        public void DrawField(SerializedProperty property) => EditorGUILayout.PropertyField(property, true);
        /// <summary>
        /// Creates a Header for the inspector
        /// </summary>
        /// <param name="label">The text for the label used in this header</param>
        public void Header(string label) => EditorGUILayout.LabelField(new GUIContent(label), EditorStyles.boldLabel);

        /// <summary>
        /// Creates a Header with a tooltip for the inspector
        /// </summary>
        /// <param name="label">The text for the label used in this header</param>
        /// <param name="tooltip">A tooltip that's displayed after the mouse hovers over the label</param>
        public void Header(string label, string tooltip) => EditorGUILayout.LabelField(new GUIContent(label, tooltip), EditorStyles.boldLabel);
    }
}
