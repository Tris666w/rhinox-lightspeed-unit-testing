using Sirenix.Utilities.Editor;
using UnityEditor;

// TODO: migrate to GUIUtils
namespace Rhinox.Utilities.Editor
{
    public static class Extensions
    {
        public static void AddItem(this GenericMenu menu, string path, GenericMenu.MenuFunction func, bool on = false)
        {
            menu.AddItem(GUIHelper.TempContent(path), on, func);
        }
    }
}