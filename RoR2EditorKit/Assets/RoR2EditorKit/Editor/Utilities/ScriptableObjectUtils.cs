using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoR2EditorKit.Utilities
{
    public static class ScriptableObjectUtils
    {
        public static void CreateNewScriptableObject<T>(Func<string> overrideName = null, Action<T> afterCreated = null) where T : ScriptableObject
        {
            ThunderKit.Core.ScriptableHelper.SelectNewAsset<T>(overrideName, afterCreated);
        }

        public static void CreateNewScriptableObject(Type t, Func<string> overrideName = null)
        {
            ThunderKit.Core.ScriptableHelper.SelectNewAsset(t, overrideName);
        }

        public static T EnsureScriptableObjectExists<T>(string assetPath, Action<T> initializer = null) where T : ScriptableObject
        {
            return ThunderKit.Core.ScriptableHelper.EnsureAsset<T>(assetPath, initializer);
        }

        public static object EnsureScriptableObjectExists(string assetPath, Type type, Action<object> initializer = null)
        {
            return ThunderKit.Core.ScriptableHelper.EnsureAsset(assetPath, type, initializer);
        }
    }
}
