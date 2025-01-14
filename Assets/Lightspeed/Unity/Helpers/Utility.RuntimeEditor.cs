using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
#endif

namespace Rhinox.Lightspeed
{
    /// <summary>
    /// Some methods that use the editor but can be used at runtime
    /// This is mostly to reduce the #IF UNITY_EDITOR in scripts
    /// </summary>
    public static partial class Utility
    {
        public static void Destroy(Object o, bool immediate = false)
        {
            if (o == null) return;
            if (immediate)
            {
                Object.DestroyImmediate(o);
                return;
            }
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(o);
            else
                Object.Destroy(o);
#else
            Object.Destroy(o);
#endif
        }
        
        public static void DestroyObject(Component t)
        {
            if (t == null) return;
            Destroy(t.gameObject);
        }

        // This function only exist to ensure that the object is destroyed; mostly to prevent errors during refactors
        public static void DestroyObject(GameObject o, bool immediate = false)
            => Destroy(o, immediate);
    
        public static bool IsEditTime()
        {
#if UNITY_EDITOR
            return !EditorApplication.isPlayingOrWillChangePlaymode;
#else
            return false;
#endif
        }
    
        public static bool IsPrefab(GameObject obj)
        {
#if UNITY_EDITOR
            return PrefabUtility.IsPartOfAnyPrefab(obj);
#else
        return false;
#endif
        }
        
        public static bool IsEditingPrefab(GameObject obj)
        {
#if UNITY_EDITOR
            return PrefabStageUtility.GetPrefabStage(obj) != null;
#else
        return false;
#endif
        }

#if UNITY_EDITOR // these functions will still require #IF UNITY_EDITOR (parameters contain this restriction)
#if UNITY_2019_1_OR_NEWER
        public static void SubscribeToSceneGui(Action<SceneView> action)
        {
            SceneView.duringSceneGui -= action;
            SceneView.duringSceneGui += action;
        }
        
        public static void UnsubscribeFromSceneGui(Action<SceneView> action)
        {
            SceneView.duringSceneGui -= action;
        }
#else
        public static void SubscribeToSceneGui(SceneView.OnSceneFunc action)
        {
            SceneView.onSceneGUIDelegate -= action;
            SceneView.onSceneGUIDelegate += action;
        }
        
        public static void UnsubscribeToSceneGui(SceneView.OnSceneFunc action)
        {
            SceneView.onSceneGUIDelegate -= action;
        }
#endif
#endif
    }
}
