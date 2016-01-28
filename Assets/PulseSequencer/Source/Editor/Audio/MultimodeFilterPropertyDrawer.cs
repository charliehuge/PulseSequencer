using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DerelictComputer
{
    [CustomPropertyDrawer(typeof(MultimodeFilter))]
    public class MultimodeFilterPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var lineHeight = EditorGUIUtility.singleLineHeight;

            var modeRect = new Rect(position.x, position.y, position.width, lineHeight);
            var modeProp = property.FindPropertyRelative("_mode");
            EditorGUI.PropertyField(modeRect, modeProp);

            var cutoffRect = new Rect(position.x, position.y + lineHeight, position.width, lineHeight);
            var cutoffProp = property.FindPropertyRelative("_cutoff");
            cutoffProp.doubleValue = EditorGUI.Slider(cutoffRect, "Cutoff (Hz)", (float)cutoffProp.doubleValue, MultimodeFilter.MinCutoff, MultimodeFilter.MaxCutoff);

            var resonanceRect = new Rect(position.x, position.y + lineHeight * 2, position.width, lineHeight);
            var resonanceProp = property.FindPropertyRelative("_resonance");
            resonanceProp.doubleValue = EditorGUI.Slider(resonanceRect, "Resonance", (float) resonanceProp.doubleValue,
                MultimodeFilter.MinResonance, MultimodeFilter.MaxResonance);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}