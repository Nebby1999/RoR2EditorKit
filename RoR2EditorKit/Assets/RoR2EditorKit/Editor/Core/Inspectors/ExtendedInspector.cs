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

    public abstract class ExtendedInspector<T> : Editor where T : Object
    {
        public static RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }

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

        protected T targetType;
        protected VisualTreeAsset visualTreeAsset;

        protected virtual void OnEnable()
        {
        }
        private void OnInspectorEnabledChange()
        {
            RootVisualElement.Clear();
            RootVisualElement.styleSheets.Clear();
            OnRootElementCleared();

            if(!InspectorEnabled)
            {
                RootVisualElement.Add(new IMGUIContainer(OnInspectorGUI));
            }
            else
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
                RootVisualElement.Add(DrawInspectorGUI());
            }
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnRootElementCleared() { }
        public override VisualElement CreateInspectorGUI()
        {
            _ = RootVisualElement;
            OnInspectorEnabledChange();
            return RootVisualElement;
        }
        protected abstract VisualElement DrawInspectorGUI();

        #region Util Methods
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

        protected void SetObjectType<TObj>(ObjectField objField) where TObj : Object
        {
            objField.objectType = typeof(TObj);
        }
        #endregion
    }
}
