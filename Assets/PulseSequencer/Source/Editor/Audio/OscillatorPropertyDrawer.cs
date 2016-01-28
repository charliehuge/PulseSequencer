using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DerelictComputer
{
    [CustomPropertyDrawer(typeof(Oscillator))]
    public class OscillatorPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 4;

            var waveformProp = property.FindPropertyRelative("_waveform");

            if (waveformProp.enumValueIndex == (int)Oscillator.WaveformType.Square)
            {
                lines = 5;
            }

            return EditorGUIUtility.singleLineHeight*lines;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var lineHeight = EditorGUIUtility.singleLineHeight;

            var waveformRect = new Rect(position.x, position.y, position.width, lineHeight);
            var waveformProp = property.FindPropertyRelative("_waveform");
            EditorGUI.PropertyField(waveformRect, waveformProp);

            var gainRect = new Rect(position.x, position.y + lineHeight, position.width, lineHeight);
            var gainProp = property.FindPropertyRelative("_gain");
            gainProp.doubleValue = EditorGUI.Slider(gainRect, "Gain", (float)gainProp.doubleValue, 0f, 1f);

            var detuneRect = new Rect(position.x, position.y + lineHeight * 2, position.width, lineHeight);
            var detuneProp = property.FindPropertyRelative("_detuneSemitones");
            detuneProp.doubleValue = EditorGUI.Slider(detuneRect, "Detune (semitones)", (float) detuneProp.doubleValue,
                -24f, 24f);

            if (waveformProp.enumValueIndex == (int)Oscillator.WaveformType.Square)
            {
                var pulseWidthRect = new Rect(position.x, position.y + lineHeight*3, position.width, lineHeight);
                var pulseWidthProp = property.FindPropertyRelative("_pulseWidth");
                pulseWidthProp.doubleValue = EditorGUI.Slider(pulseWidthRect, "Pulse Width",
                    (float) pulseWidthProp.doubleValue, 0.1f, 0.9f);
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}