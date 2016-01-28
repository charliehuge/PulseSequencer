using UnityEngine;
using System.Collections;
using UnityEditor;
#if OFF
namespace DerelictComputer
{
    [CustomEditor(typeof(SubtractiveSynth))]
    public class SubtractiveSynthInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var synth = (SubtractiveSynth) target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            var oscillators = serializedObject.FindProperty("_oscillators");

            for (int i = 0; i < oscillators.arraySize; i++)
            {
                var oscillator = oscillators.GetArrayElementAtIndex(i);
                EditorGUILayout.LabelField("Oscillator " + (i + 1));
                EditorGUILayout.PropertyField(oscillator);
            }

            EditorGUILayout.LabelField("Envelope");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_envelope"));

            EditorGUILayout.LabelField("Filter");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_filter"));

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
#endif