using UnityEngine;
using UnityEditor;
public class TimeEditor : EditorWindow
{
    float time;
    TouchGazeManager.InteractType interact;

    // Add menu named "My Window" to the Window menu
    [MenuItem("OTM/TimeTester")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        TimeEditor window = (TimeEditor)EditorWindow.GetWindow(typeof(TimeEditor));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        time = EditorGUILayout.FloatField("TimeToAddInSeconds", time);
        interact = (TouchGazeManager.InteractType)EditorGUILayout.EnumPopup("interact type", interact);
        if (GUILayout.Button("AddTime"))
        {
            TouchGazeManager.Instance.SetTime(interact, time);
        }
    }
}