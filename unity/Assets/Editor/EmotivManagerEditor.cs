using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EmotivManager))]
public class EmotivManagerEditor : Editor
{
    SerializedProperty selectedHeadsetIndexProperty;
    SerializedProperty headsetNamesProperty;

    private void OnEnable()
    {
        selectedHeadsetIndexProperty = serializedObject.FindProperty("selectedHeadsetIndex");
        headsetNamesProperty = serializedObject.FindProperty("headsetNames");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Select Headset Name", EditorStyles.boldLabel);

        string[] headsetOptions = new string[headsetNamesProperty.arraySize];
        for (int i = 0; i < headsetNamesProperty.arraySize; i++)
        {
            headsetOptions[i] = headsetNamesProperty.GetArrayElementAtIndex(i).stringValue;
        }

        if (headsetOptions.Length > 0)
        {
            int currentIndex = selectedHeadsetIndexProperty.intValue;
            int newIndex = EditorGUILayout.Popup("Headset Name", currentIndex, headsetOptions);

            if (newIndex != currentIndex)
            {
                selectedHeadsetIndexProperty.intValue = newIndex;
                serializedObject.ApplyModifiedProperties();

                // Assuming EmotivManager has a public method to handle headset selection changes
                EmotivManager manager = (EmotivManager)target;
                manager.selectedHeadsetIndex = newIndex;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No headset names defined.", MessageType.Info);
        }

        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
