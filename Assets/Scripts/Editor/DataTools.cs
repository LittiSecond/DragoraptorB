using UnityEngine;
using UnityEditor;


namespace Dragoraptor.Editor
{
    public class DataTools : EditorWindow
    {
        private const string CAPTION = "    Functional to data (inport, export)";

        private void OnGUI()
        {
            GUILayout.Space(20);
            GUILayout.Label(CAPTION);
            GUILayout.Space(20);
        }
        
        [MenuItem("CustomTools/Data tools")]
        public static void ShowCustomTools()
        {
            EditorWindow.GetWindow(typeof(DataTools));
        }
    }
}