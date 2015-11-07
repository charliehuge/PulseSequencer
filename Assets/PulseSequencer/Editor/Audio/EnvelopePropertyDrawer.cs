using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DerelictComputer
{
    [CustomPropertyDrawer(typeof(Envelope))]
    public class EnvelopePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 5;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var lineHeight = EditorGUIUtility.singleLineHeight;

            var attackTimeRect = new Rect(position.x, position.y, position.width, lineHeight);
            var attackTimeProp = property.FindPropertyRelative("_attackTime");
            attackTimeProp.doubleValue = EditorGUI.Slider(attackTimeRect, "Attack Time", (float)attackTimeProp.doubleValue, 0f, 4f);

            var decayTimeRect = new Rect(position.x, position.y + lineHeight, position.width, lineHeight);
            var decayTimeProp = property.FindPropertyRelative("_decayTime");
            decayTimeProp.doubleValue = EditorGUI.Slider(decayTimeRect, "Decay Time", (float)decayTimeProp.doubleValue, 0f, 4f);

            var sustainRect = new Rect(position.x, position.y + lineHeight * 2, position.width, lineHeight);
            var sustainProp = property.FindPropertyRelative("_sustainLevel");
            sustainProp.doubleValue = EditorGUI.Slider(sustainRect, "Sustain Level", (float) sustainProp.doubleValue,
                0f, 1f);

            var releaseTimeRect = new Rect(position.x, position.y + lineHeight * 3, position.width, lineHeight);
            var releaseTimeProp = property.FindPropertyRelative("_releaseTime");
            releaseTimeProp.doubleValue = EditorGUI.Slider(releaseTimeRect, "Release Time", (float)releaseTimeProp.doubleValue, 0f, 4f);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}